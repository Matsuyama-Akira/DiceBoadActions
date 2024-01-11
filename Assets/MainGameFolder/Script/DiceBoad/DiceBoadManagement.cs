using UnityEngine;
using NextSceneScript;
using AllGameManager;
using System.Collections;

public class DiceBoadManagement : MonoBehaviour
{
    /// <summary> マスの種類 </summary>
    public enum CurrentMassStatus
    {
        Normal, Items, Heal, Enemy1, Enemy2, Boss,
    }

    // 必須スクリプト
    private NextScene chengeScene;
    private WeponSellect wepon;
    private AllGameStates playerStatus;
    private EnemySellect enemy;
    private AllGameSEManager seManager;

    [Header("Charactor")]
    [SerializeField, Tooltip("プレイヤーがスタートするマスのトランスフォーム")] Transform startPosition;
    [SerializeField, Tooltip("プレイヤーオブジェクト")] GameObject playerObject;
    [SerializeField, Tooltip("プレイヤーのインスタンス")] GameObject _player;
    [SerializeField, Tooltip("プレイヤーのトランスフォーム")] Transform player;
    [SerializeField, Tooltip("プレイヤーのアニメーター")] Animator playerAnimator;
    [SerializeField, Range(5f, 100f), Tooltip("移動速度")] float moveSpeed = 5.0f;

    [Space, Header("Dice")]
    [SerializeField, Tooltip("サイコロのオブジェクト")] GameObject[] DiceObject;
    [SerializeField, Tooltip("サイコロの生成地点")] Transform DiceSpawnPosition;
    [SerializeField, Tooltip("サイコロのインスタンス")] GameObject _dice;
    [SerializeField, Tooltip("サイコロのスクリプト")] Dice diceScript;

    [Space, Header("MapEffect")]
    [SerializeField, Tooltip("回復のエフェクト")] GameObject healEffect;

    [Space, Header("ClearList")]
    [SerializeField, Tooltip("すごろくのクリアリスト")] DB_ClearList clearList;

    //Private status
    /// <summary> 移動できる数 </summary>
    private int movePoint;
    /// <summary> 経過したターン数 </summary>
    public bool playerTurn { get; private set; }
    /// <summary> 待機中 </summary>
    private bool isStandby;
    /// <summary> 移動中 </summary>
    private bool isMoving;
    /// <summary> 移動後 </summary>
    private bool isMoved = true;
    /// <summary> 対象へ移動を終えたか </summary>
    public bool moveOut { get; private set; }
    /// <summary> 次の移動先 </summary>
    private Vector3 nextPosition;
    /// <summary> 横や後ろへの移動時の回転 </summary>
    private Quaternion nextQuaternion;
    /// <summary> 移動先が移動できるか </summary>
    private bool[] moveChecks = new bool[4];
    /// <summary> 移動可能方向への矢印オブジェクト </summary>
    private GameObject[] Arrow = new GameObject[4];
    /// <summary> 次のマスのトランスフォーム </summary>
    private Transform[] nextMass = new Transform[4];
    /// <summary> 現在立っているマスの種類 </summary>
    private CurrentMassStatus nowMass;
    /// <summary> クリアしたか </summary>
    private bool lateClear;
    /// <summary> クリアしたマスか </summary>
    public static bool[,] clearMass = new bool[11, 11];
    /// <summary> 新しい武器の種類とレアリティ </summary>
    private int[] newWepons = new int[2];
    /// <summary> 新しい武器を選んだか </summary>
    private bool isWeponSellect;
    /// <summary> 回復したか </summary>
    private bool healing;
    /// <summary> 宝箱を開けたか </summary>
    private bool itemBoxOpening;
    /// <summary> 敵を倒したか </summary>
    static private bool enemyDown;

    // キー入力を流用
    public bool flont { get; private set; }
    public bool back { get; private set; }
    public bool right { get; private set; }
    public bool left { get; private set; }
    public bool jump { get; private set; }
    public bool run { get; private set; }
    public bool attack { get; private set; }
    public bool unique { get; private set; }
    public bool skill { get; private set; }

    void Awake()
    {
        // セットアップ
        GameObject _manager = GameObject.FindWithTag("GameManager");
        wepon = _manager.GetComponent<WeponSellect>();
        playerStatus = _manager.GetComponent<AllGameStates>();
        seManager = _manager.GetComponent<AllGameSEManager>();
        enemy = _manager.GetComponent<EnemySellect>();
        chengeScene = GetComponent<NextScene>();

        // ステータスの初期化
        Cursor.lockState = CursorLockMode.None;
        isMoving = false;
        isMoved = true;

        // ゲームスタート直後か
        if (playerStatus.GetStart())
        {
            // プレイヤーを前回の地点に生成
            _player = Instantiate(playerObject, playerStatus.GetLatePosition(), playerStatus.GetLateQuaternion());
        }
        else
        {
            // ステータスの初期化
            healing = false;
            itemBoxOpening = false;
            enemyDown = false;
            playerStatus.ResetHP();
            playerStatus.AddPlayerLateHP();

            // プレイヤーを初期スポーン地点に生成
            _player = Instantiate(playerObject, startPosition);
            playerStatus.AddStart();
        }

        // 生成したプレイヤーからステータスを取得
        playerAnimator = _player.GetComponentInChildren<Animator>();
        nextPosition = _player.transform.position;
        lateClear = playerStatus.GetLateClear();

        // 進行方向の矢印オブジェクトを取得
        Arrow[0] = GameObject.Find("Arrow_F");
        Arrow[1] = GameObject.Find("Arrow_B");
        Arrow[2] = GameObject.Find("Arrow_R");
        Arrow[3] = GameObject.Find("Arrow_L");
    }

    void Update()
    {
        MassMove();
        GamePlay();
    }

    void MassMove()
    {
        switch (nowMass)
        {
            // Normalマス　何もしない
            case CurrentMassStatus.Normal:
                if (!isMoving & moveOut)
                {
                    AddSerialBools();
                    isMoved = true;
                }
                break;
            // Itemsマス　宝箱を開く　SEを再生　その後武器選択
            case CurrentMassStatus.Items:
                if (!isMoving & moveOut)
                {
                    AddSerialBools();
                    itemBoxOpening = true;
                    if (seManager.DB_MassSE[1] != null & !seManager.DB_MassSESource.isPlaying)
                        seManager.DB_MassSESource.PlayOneShot(seManager.DB_MassSE[1]);
                    isMoved = true;
                }
                break;
            // Healマス　プレイヤーのHPを最大値に回復　エフェクトを生成　SEを再生
            case CurrentMassStatus.Heal:
                if (!isMoving & !isMoved & moveOut)
                {
                    AddSerialBools();
                    healing = true;
                    playerStatus.AddPlayerLateHP();
                    playerStatus.AddHealHP(999);
                    Instantiate(healEffect, player);
                    if (seManager.DB_MassSE[2] != null) seManager.DB_MassSESource.PlayOneShot(seManager.DB_MassSE[2]);
                    isMoved = true;
                }
                break;
            // Enemy1マス　敵のレベルをセット　バトルシーンへ
            case CurrentMassStatus.Enemy1:
                if (!isMoving & !isMoved & moveOut)
                {
                    AddSerialBools();
                    enemyDown = true;
                    enemy.AddLevel(1);
                    SceneChenge("BattleScene");
                }
                break;
            // Enemy2マス　敵のレベルをセット　バトルシーンへ
            case CurrentMassStatus.Enemy2:
                if (!isMoving & !isMoved & moveOut)
                {
                    AddSerialBools();
                    enemyDown = true;
                    enemy.AddLevel(2);
                    SceneChenge("BattleScene");
                }
                break;
            // Bossマス　敵のレベルをセット　バトルシーンへ
            case CurrentMassStatus.Boss:
                enemy.AddLevel(3);
                SceneChenge("BattleScene");
                break;
        }
    }

    void GamePlay()
    {
        // Boolや入力の管理
        Manager();

        // プレイヤーが動かせるなら
        if (playerTurn)
        {
            // サイコロを生成していない時にSkillキーを入力したら
            if (skill & _dice == null)
            {
                // 経過ターンを増やす
                playerStatus.AddPlayTurn();

                // ランダムな色のサイコロを生成し、スクリプトを取得してSEを再生
                int random = Random.Range(0, 5);
                _dice = Instantiate(DiceObject[random], DiceSpawnPosition.position, DiceObject[random].transform.rotation);
                diceScript = _dice.GetComponentInChildren<Dice>();
                if (seManager.DB_AnySE[1] != null & seManager.DB_MassSESource != null) seManager.DB_MassSESource.PlayOneShot(seManager.DB_AnySE[1]);
            }
            // サイコロのスクリプトがNullではない場合
            if (diceScript != null)
            {
                // サイコロが静止したら
                if (diceScript.GetIsStoping())
                {
                    // サイコロの結果を取得し、プレイヤーを動かす
                    movePoint = diceScript.GetResult();
                    isMoved = false;
                    lateClear = false;
                    Destroy(_dice);
                    StartCoroutine(Move());
                }
            }
        }

        // アニメーションの再生
        MoveAnim();
    }

    private IEnumerator Move()
    {
        // 移動中
        isMoving = true;
        while (movePoint > 0)
        {
            PlayerMove();
            yield return null;
        }
        isMoving = false;
    }

    void PlayerMove()
    {
        if (movePoint > 0 & moveOut)
        {
            if (flont & moveChecks[0])
            {
                nextPosition = nextMass[0].position;
                nextQuaternion = Quaternion.AngleAxis(0, Vector3.up);
                player.localRotation *= nextQuaternion;
                int randomNum = Random.Range(0, seManager.DB_FootSE.Length);
                if (seManager.DB_FootSE[randomNum] != null) seManager.DB_FootSESource.PlayOneShot(seManager.DB_FootSE[randomNum]);
                movePoint--;
                return;
            }
            if (back & moveChecks[1])
            {
                nextPosition = nextMass[1].position;
                nextQuaternion = Quaternion.AngleAxis(180, Vector3.up);
                player.localRotation *= nextQuaternion;
                int randomNum = Random.Range(0, seManager.DB_FootSE.Length);
                if (seManager.DB_FootSE[randomNum] != null) seManager.DB_FootSESource.PlayOneShot(seManager.DB_FootSE[randomNum]);
                movePoint--;
                return;
            }
            if (right & moveChecks[2])
            {
                nextPosition = nextMass[2].position;
                nextQuaternion = Quaternion.AngleAxis(90, Vector3.up);
                player.localRotation *= nextQuaternion;
                int randomNum = Random.Range(0, seManager.DB_FootSE.Length);
                if (seManager.DB_FootSE[randomNum] != null) seManager.DB_FootSESource.PlayOneShot(seManager.DB_FootSE[randomNum]);
                movePoint--;
                return;
            }
            if (left & moveChecks[3])
            {
                nextPosition = nextMass[3].position;
                nextQuaternion = Quaternion.AngleAxis(-90, Vector3.up);
                player.localRotation *= nextQuaternion;
                int randomNum = Random.Range(0, seManager.DB_FootSE.Length);
                if (seManager.DB_FootSE[randomNum] != null) seManager.DB_FootSESource.PlayOneShot(seManager.DB_FootSE[randomNum]);
                movePoint--;
                return;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void MoveAnim()
    {
        moveOut = player.position == nextPosition;
        playerAnimator.SetBool("Moving", !moveOut);
        player.position = Vector3.MoveTowards(player.position, nextPosition, moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 
    /// </summary>
    void Manager()
    {
        KeyInput();
        playerTurn = !isMoving & !healing & !isWeponSellect & !itemBoxOpening & !enemyDown;
        player = _player.transform;
        isStandby = !isMoving & _dice == null;

        int arrow = 0;
        while (arrow < 4)
        {
            Arrow[arrow].SetActive(moveChecks[arrow] & isMoving);
            arrow++;
        }
    }

    void KeyInput()
    {
        flont = Input.GetKeyDown(Controller.Flont);
        back = Input.GetKeyDown(Controller.Back);
        right = Input.GetKeyDown(Controller.Right);
        left = Input.GetKeyDown(Controller.Left);
        jump = Input.GetKeyDown(Controller.Jump);
        run = Input.GetKeyDown(Controller.Run);
        attack = Input.GetKeyDown(Controller.Attack);
        unique = Input.GetKeyDown(Controller.Unique);
        skill = Input.GetKeyDown(Controller.Skill);
    }

    void Debugger()
    {
        int num1 = 0, num2;
        while (num1 < 11)
        {
            num2 = 0;
            while (num2 < 11)
            {
                if (clearMass[num1, num2])
                {
                    Debug.LogWarning("(" + num1 + "," + num2 + ") = true");
                }
                num2++;
            }
            num1++;
        }
    }

    void AddSerialBools()
    {
        int num1 = 0;
        int num2;
        while (num1 < 11)
        {
            num2 = 0;
            while (num2 < 11)
            {
                clearList.GetBools(num1, num2, clearMass[num1, num2]);
                num2++;
            }
            num1++;
        }
    }

    void SceneChenge(string sceneName)
    {
        playerStatus.AddPosition(player.position);
        playerStatus.AddQuaternion(player.rotation);
        chengeScene.ChengeScene(sceneName);
    }

    public void AddEventMass() { playerStatus.AddEventMass(); }

    public int GetMovePoint() { return movePoint; }
    public bool GetLateClaer() { return lateClear; }
    public bool GetIsStandby() { return isStandby; }
    public bool GetIsMoved() { return isMoved; }
    public bool GetClearMass(int num1, int num2) { return clearMass[num1, num2]; }
    public int GetNewWepons(int SelORLev) { return newWepons[SelORLev]; }
    public bool GetIsWeponSellect() { return isWeponSellect; }

    public void SetMoveCheck(int num, bool check) { moveChecks[num] = check; }
    public void SetNextMass(int num, Transform next) { nextMass[num] = next; }
    public void SetLateClear(bool late) { lateClear = late; }
    public void SetHealing(bool heal) { healing = heal; }
    public void SetEnemyDown(bool down) { enemyDown = down; }
    public void SetClearMass(int num1, int num2, bool isClear) { clearMass[num1, num2] = isClear; }
    public void SetNewWepons(int weponSellection, int Level) { newWepons[0] = weponSellection; newWepons[1] = Level; isWeponSellect = true; }

    public void CurrentMass(int massStatus)
    {
        switch (massStatus)
        {
            case 0: nowMass = CurrentMassStatus.Normal; break; //Normal
            case 1: nowMass = CurrentMassStatus.Items; break; //Items
            case 2: nowMass = CurrentMassStatus.Heal; break; //Heal
            case 3: nowMass = CurrentMassStatus.Enemy1; break; //Enemy1
            case 4: nowMass = CurrentMassStatus.Enemy2; break; //Enemy2
            case 5: nowMass = CurrentMassStatus.Boss; break; //Boss
        }
    }

    public void OnWeponSellectedBotton(bool chenge)
    {
        if (chenge)
        {
            switch (newWepons[0])
            {
                case 0: wepon.wepon = WeponSellect.Wepon.Sword; break;
                case 1: wepon.wepon = WeponSellect.Wepon.Spear; break;
                case 2: wepon.wepon = WeponSellect.Wepon.Bow; break;
                case 3: wepon.wepon = WeponSellect.Wepon.Gun; break;
                case 4: wepon.wepon = WeponSellect.Wepon.Magic; break;
            }
            switch (newWepons[1])
            {
                case 0: wepon.rarelity = WeponSellect.Rarelity.Common; break;
                case 1: wepon.rarelity = WeponSellect.Rarelity.Rare; break;
                case 2: wepon.rarelity = WeponSellect.Rarelity.Unique; break;
            }
            playerStatus.AddHealHP(0);
        }
        isWeponSellect = false;
        itemBoxOpening = false;
    }
}

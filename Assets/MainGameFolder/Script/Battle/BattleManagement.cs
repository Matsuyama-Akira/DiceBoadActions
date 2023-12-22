using UnityEngine;
using NextSceneScript;
using AllGameManager;
using Unity.AI.Navigation;

public class BattleManagement : MonoBehaviour
{
    // 必須スクリプト
    private AllGameManagement manager;
    private WeponSellect weponSellect;
    private EnemySellect enemySellect;
    private AllGameStates AllStatus;
    [SerializeField] NextScene sceneChenge;
    [SerializeField] BattleUIManagement uiManager;

    /// <summary> Bowが再度打てるようになるまでのゲージUI </summary>
    [SerializeField] GameObject reroadUI;

    // キャラクターの生成
    [NamedArray(new string[] {"Sword", "Spire", "Bow", "Gun", "Magic"}), SerializeField, Tooltip("プレイヤーのデータ")] GameObject[] Player;
    [SerializeField, Tooltip("プレイヤーのスポーン地点")] GameObject PlayerSpawnPoint;
    [NamedArray(new string[] {"Enemy1", "Enemy2", "Boss"}), SerializeField, Tooltip("敵のデータ")] GameObject[] Enemy;
    [SerializeField, Tooltip("敵のスポーン地点")] GameObject EnemySpawnPoint;

    // キャラクターデータの保持
    /// <summary> プレイヤーのオブジェクト </summary>
    private GameObject _player;
    /// <summary> 敵のオブジェクト </summary>
    private GameObject _enemy;
    /// <summary> プレイヤーのステータス </summary>
    private PlayerStatus player;
    /// <summary> 敵のステータス </summary>
    private EnemyStates enemy;

    [SerializeField, Tooltip("NavMeshの生成")] NavMeshSurface mapNavMesh;
    [SerializeField, Tooltip("このバトルで与えたダメージ")] int allDamage;

    /// <summary> バトルが開始した時間 </summary>
    private float startTime;
    /// <summary> バトルが開始してからの経過時間 </summary>
    private float nowTime;

    public void AddAllDamage(int damage) { allDamage += damage; }
    public int GetAllDamage() { return allDamage; }

    /// <summary>
    /// セットアップ
    /// </summary>
    void Awake()
    {
        // カーソルを非表示
        Cursor.lockState = CursorLockMode.Locked;

        // 必要なスクリプトのキャッシュ
        GameObject managerObject = GameObject.FindWithTag("GameManager");
        AllStatus = managerObject.GetComponent<AllGameStates>();
        manager = managerObject.GetComponent<AllGameManagement>();
        weponSellect = managerObject.GetComponent<WeponSellect>();
        enemySellect = managerObject.GetComponent<EnemySellect>();

        // キャラクターの生成
        _player = Instantiate(Player[(int)weponSellect.wepon], PlayerSpawnPoint.transform.position, PlayerSpawnPoint.transform.rotation);
        _enemy = Instantiate(Enemy[(int)enemySellect.GetLevel()], EnemySpawnPoint.transform.position, EnemySpawnPoint.transform.rotation);
        player = _player.GetComponent<PlayerStatus>();
        enemy = _enemy.GetComponent<EnemyStates>();

        // UIのセットアップ
        uiManager.SetStatus(player);
        if (weponSellect.wepon != WeponSellect.Wepon.Bow) reroadUI.SetActive(false);

        // バトルデータのリセット
        allDamage = 0;
        startTime = Time.time;
    }

    void Update()
    {
        IfStates();
        UIManagement();
        SetNowTime();
        ResetNavMesh();
    }

    /// <summary>
    /// 条件を満たしたらシーンを切り替える
    /// </summary>
    void IfStates()
    {
        // 敵のHPが0になったら
        if (enemy.isDieAnimEnd)
        {
            // バトルのデータを送信
            manager.AddBattleResult(0);
            AllStatus.AddPlayerHP(player.GetNowHP());
            AllStatus.AddPlayerLateHP();
            AllStatus.AddKillCount(1);
            AllStatus.AddAllDamage(allDamage);

            // ボスを倒したらリザルトへ
            if (enemySellect.GetLevel() == EnemySellect.Level.Boss) sceneChenge.ChengeScene("ResultScene");
            else
            {
                sceneChenge.ChengeScene("DiceBoadScene");
                AllStatus.AddLateClear(true);
            }

            // カーソルの再表示
            Cursor.lockState = CursorLockMode.Confined;
        }

        // プレイヤーのHPが0になったら
        if (player.isDie)
        {
            Debugger();

            // 敗北のリザルトへ
            manager.AddBattleResult(1);
            sceneChenge.ChengeScene("ResultScene");

            // カーソルの再表示
            Cursor.lockState = CursorLockMode.Confined;
        }

        // Escapeを入力したらカーソルの再表示
        if(Input.GetKey(KeyCode.Escape)) Cursor.lockState = CursorLockMode.Confined;
    }

    /// <summary>
    /// 経過時間をセット
    /// </summary>
    void SetNowTime()
    {
        nowTime = Time.time - startTime;
    }

    /// <summary>
    /// UI制御
    /// </summary>
    void UIManagement()
    {
        uiManager.TimeTextUI(nowTime);
        uiManager.PlayerHPUI((float)player.GetNowHP() / AllStatus.GetWeponHP(), (float)player.GetLateHP() / AllStatus.GetWeponHP());
        uiManager.MoveUI();
    }

    /// <summary>
    /// NavMeshの再生成
    /// </summary>
    void ResetNavMesh()
    {
        mapNavMesh.BuildNavMesh();
    }

    void Debugger()
    {
        Debug.LogError(player.GetNowHP());
    }
}

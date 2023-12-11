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

    /// <summary> Bow用のリロードUI </summary>
    [SerializeField] GameObject reroadUI;

    // キャラクターの生成用データ
    /// <summary> 武器毎のプレイヤーオブジェクト </summary>
    [NamedArray(new string[] {"Sword", "Spire", "Bow", "Gun", "Magic"}), SerializeField] GameObject[] Player;
    /// <summary> プレイヤーのスポーンポイント </summary>
    [SerializeField] GameObject PlayerSpawnPoint;
    /// <summary> レベル毎の敵オブジェクト </summary>
    [NamedArray(new string[] {"Enemy1", "Enemy2", "Boss"}), SerializeField] GameObject[] Enemy;
    /// <summary> 敵のスポーンポイント </summary>
    [SerializeField] GameObject EnemySpawnPoint;

    // 生成したキャラクターのデータ管理
    /// <summary> 生成したプレイヤー </summary>
    private GameObject _player;
    /// <summary> 生成した敵 </summary>
    private GameObject _enemy;
    /// <summary> 生成したプレイヤーステータス </summary>
    private PlayerStatus player;
    /// <summary> 生成した敵ステータス </summary>
    private EnemyStates enemy;

    /// <summary> マップのNavMeshデータ </summary>
    [SerializeField] NavMeshSurface mapNavMesh;
    /// <summary> プレイヤーが与えたダメージ </summary>
    [SerializeField] int allDamage;

    /// <summary> バトルシーンに入った時間を0とするための変数 </summary>
    private float startTime;
    /// <summary> 0とした時間からの経過時間 </summary>
    private float nowTime;

    public void AddAllDamage(int damage) { allDamage += damage; }
    public int GetAllDamage() { return allDamage; }

    /// <summary>
    /// アセットの読み込み等の立ち上げ
    /// </summary>
    void Awake()
    {
        // カーソルを非表示
        Cursor.lockState = CursorLockMode.Locked;

        // 必要なコンポーネントのキャッシュ
        GameObject managerObject = GameObject.FindWithTag("GameManager");
        AllStatus = managerObject.GetComponent<AllGameStates>();
        manager = managerObject.GetComponent<AllGameManagement>();
        weponSellect = managerObject.GetComponent<WeponSellect>();
        enemySellect = managerObject.GetComponent<EnemySellect>();

        // オブジェクトの生成とステータスのキャッシュ
        _player = Instantiate(Player[(int)weponSellect.wepon], PlayerSpawnPoint.transform.position, PlayerSpawnPoint.transform.rotation);
        _enemy = Instantiate(Enemy[(int)enemySellect.GetLevel()], EnemySpawnPoint.transform.position, EnemySpawnPoint.transform.rotation);
        player = _player.GetComponent<PlayerStatus>();
        enemy = _enemy.GetComponent<EnemyStates>();

        // UIの読み込み
        uiManager.SetStatus(player);
        if (weponSellect.wepon != WeponSellect.Wepon.Bow) reroadUI.SetActive(false);

        // データのリセット
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
    /// 条件を満たしたらシーン切り替え等の処理を行う
    /// </summary>
    void IfStates()
    {
        // 敵のHPが0になったら
        if (enemy.isDieAnimEnd)
        {
            // 必要なデータの受け渡し
            manager.AddBattleResult(0);
            AllStatus.AddPlayerHP(player.GetNowHP());
            AllStatus.AddPlayerLateHP();
            AllStatus.AddKillCount(1);
            AllStatus.AddAllDamage(allDamage);

            // 倒した敵がボスの場合はリザルトへ
            if (enemySellect.GetLevel() == EnemySellect.Level.Boss) sceneChenge.ChengeScene("ResultScene");
            else
            {
                sceneChenge.ChengeScene("DiceBoadScene");
                AllStatus.AddLateClear(true);
            }

            // カーソルの表示
            Cursor.lockState = CursorLockMode.Confined;
        }

        // プレイヤーのHPが0になったら
        if (player.isDie)
        {
            // 必要なデータの受け渡し
            manager.AddBattleResult(1);
            sceneChenge.ChengeScene("ResultScene");

            // カーソルの表示
            Cursor.lockState = CursorLockMode.Confined;
        }

        // Escapeキーを入力したらカーソルの表示
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
    /// UIを動かす
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
}

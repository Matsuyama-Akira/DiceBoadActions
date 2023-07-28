using UnityEngine;
using NextSceneScript;
using AllGameManager;
using Unity.AI.Navigation;

public class BattleManagement : MonoBehaviour
{
    private AllGameManagement manager;
    private WeponSellect weponSellect;
    private EnemySellect enemySellect;
    private AllGameStates AllStatus;
    [SerializeField] BattleUIManagement uiManager;
    [SerializeField] GameObject reroadUI;
    [NamedArray(new string[] {"Sword", "Spire", "Bow", "Gun", "Magic"})]
    [SerializeField] GameObject[] Player;
    [SerializeField] GameObject PlayerSpawnPoint;
    [NamedArray(new string[] {"Enemy1", "Enemy2", "Boss"})]
    [SerializeField] GameObject[] Enemy;
    [SerializeField] GameObject EnemySpawnPoint;
    private GameObject _player;
    private GameObject _enemy;
    private EnemyStates enemy;
    private PlayerStatus player;
    [SerializeField] NextScene sceneChenge;
    [SerializeField] NavMeshSurface mapNavMesh;
    [SerializeField] int allDamage;

    private float startTime;
    private float nowTime;

    public void AddAllDamage(int damage) { allDamage += damage; }
    public int GetAllDamage() { return allDamage; }

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameObject managerObject = GameObject.FindWithTag("GameManager");
        AllStatus = managerObject.GetComponent<AllGameStates>();
        manager = managerObject.GetComponent<AllGameManagement>();
        weponSellect = managerObject.GetComponent<WeponSellect>();
        enemySellect = managerObject.GetComponent<EnemySellect>();
        _player = Instantiate(Player[(int)weponSellect.wepon], PlayerSpawnPoint.transform.position, PlayerSpawnPoint.transform.rotation);
        _enemy = Instantiate(Enemy[(int)enemySellect.GetLevel()], EnemySpawnPoint.transform.position, EnemySpawnPoint.transform.rotation);
        player = _player.GetComponent<PlayerStatus>();
        enemy = _enemy.GetComponent<EnemyStates>();
        uiManager.SetStatus(player);
        if (weponSellect.wepon != WeponSellect.Wepon.Bow) reroadUI.SetActive(false);
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
    void IfStates()
    {
        if (enemy.isDieAnimEnd)
        {
            manager.AddBattleResult(0);
            AllStatus.AddPlayerHP(player.GetNowHP());
            AllStatus.AddKillCount(1);
            AllStatus.AddAllDamage(allDamage);
            if (enemySellect.GetLevel() == EnemySellect.Level.Boss) sceneChenge.ChengeScene("ResultScene");
            else
            {
                sceneChenge.ChengeScene("DiceBoadScene");
                AllStatus.AddLateClear(true);
            }
            Cursor.lockState = CursorLockMode.Confined;
        }
        if (player.isDie)
        {
            manager.AddBattleResult(1);
            sceneChenge.ChengeScene("ResultScene");
            Cursor.lockState = CursorLockMode.Confined;
        }
        if(Input.GetKey(KeyCode.Escape)) Cursor.lockState = CursorLockMode.Confined;
    }
    void SetNowTime()
    {
        nowTime = Time.time - startTime;
    }

    void UIManagement()
    {
        uiManager.TimeTextUI(nowTime);
        uiManager.PlayerHPUI((float)player.GetNowHP() / AllStatus.GetWeponHP(), (float)player.GetLateHP() / AllStatus.GetWeponHP());
        uiManager.MoveUI();
    }

    void ResetNavMesh()
    {
        mapNavMesh.BuildNavMesh();
    }
}

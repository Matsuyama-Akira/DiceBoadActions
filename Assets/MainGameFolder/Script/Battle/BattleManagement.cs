using UnityEngine;
using NextSceneScript;
using AllGameManager;
using Unity.AI.Navigation;

public class BattleManagement : MonoBehaviour
{
    // �K�{�X�N���v�g
    private AllGameManagement manager;
    private WeponSellect weponSellect;
    private EnemySellect enemySellect;
    private AllGameStates AllStatus;
    [SerializeField] NextScene sceneChenge;
    [SerializeField] BattleUIManagement uiManager;

    /// <summary> Bow�p�̃����[�hUI </summary>
    [SerializeField] GameObject reroadUI;

    // �L�����N�^�[�̐����p�f�[�^
    /// <summary> ���했�̃v���C���[�I�u�W�F�N�g </summary>
    [NamedArray(new string[] {"Sword", "Spire", "Bow", "Gun", "Magic"}), SerializeField] GameObject[] Player;
    /// <summary> �v���C���[�̃X�|�[���|�C���g </summary>
    [SerializeField] GameObject PlayerSpawnPoint;
    /// <summary> ���x�����̓G�I�u�W�F�N�g </summary>
    [NamedArray(new string[] {"Enemy1", "Enemy2", "Boss"}), SerializeField] GameObject[] Enemy;
    /// <summary> �G�̃X�|�[���|�C���g </summary>
    [SerializeField] GameObject EnemySpawnPoint;

    // ���������L�����N�^�[�̃f�[�^�Ǘ�
    /// <summary> ���������v���C���[ </summary>
    private GameObject _player;
    /// <summary> ���������G </summary>
    private GameObject _enemy;
    /// <summary> ���������v���C���[�X�e�[�^�X </summary>
    private PlayerStatus player;
    /// <summary> ���������G�X�e�[�^�X </summary>
    private EnemyStates enemy;

    /// <summary> �}�b�v��NavMesh�f�[�^ </summary>
    [SerializeField] NavMeshSurface mapNavMesh;
    /// <summary> �v���C���[���^�����_���[�W </summary>
    [SerializeField] int allDamage;

    /// <summary> �o�g���V�[���ɓ��������Ԃ�0�Ƃ��邽�߂̕ϐ� </summary>
    private float startTime;
    /// <summary> 0�Ƃ������Ԃ���̌o�ߎ��� </summary>
    private float nowTime;

    public void AddAllDamage(int damage) { allDamage += damage; }
    public int GetAllDamage() { return allDamage; }

    /// <summary>
    /// �A�Z�b�g�̓ǂݍ��ݓ��̗����グ
    /// </summary>
    void Awake()
    {
        // �J�[�\�����\��
        Cursor.lockState = CursorLockMode.Locked;

        // �K�v�ȃR���|�[�l���g�̃L���b�V��
        GameObject managerObject = GameObject.FindWithTag("GameManager");
        AllStatus = managerObject.GetComponent<AllGameStates>();
        manager = managerObject.GetComponent<AllGameManagement>();
        weponSellect = managerObject.GetComponent<WeponSellect>();
        enemySellect = managerObject.GetComponent<EnemySellect>();

        // �I�u�W�F�N�g�̐����ƃX�e�[�^�X�̃L���b�V��
        _player = Instantiate(Player[(int)weponSellect.wepon], PlayerSpawnPoint.transform.position, PlayerSpawnPoint.transform.rotation);
        _enemy = Instantiate(Enemy[(int)enemySellect.GetLevel()], EnemySpawnPoint.transform.position, EnemySpawnPoint.transform.rotation);
        player = _player.GetComponent<PlayerStatus>();
        enemy = _enemy.GetComponent<EnemyStates>();

        // UI�̓ǂݍ���
        uiManager.SetStatus(player);
        if (weponSellect.wepon != WeponSellect.Wepon.Bow) reroadUI.SetActive(false);

        // �f�[�^�̃��Z�b�g
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
    /// �����𖞂�������V�[���؂�ւ����̏������s��
    /// </summary>
    void IfStates()
    {
        // �G��HP��0�ɂȂ�����
        if (enemy.isDieAnimEnd)
        {
            // �K�v�ȃf�[�^�̎󂯓n��
            manager.AddBattleResult(0);
            AllStatus.AddPlayerHP(player.GetNowHP());
            AllStatus.AddPlayerLateHP();
            AllStatus.AddKillCount(1);
            AllStatus.AddAllDamage(allDamage);

            // �|�����G���{�X�̏ꍇ�̓��U���g��
            if (enemySellect.GetLevel() == EnemySellect.Level.Boss) sceneChenge.ChengeScene("ResultScene");
            else
            {
                sceneChenge.ChengeScene("DiceBoadScene");
                AllStatus.AddLateClear(true);
            }

            // �J�[�\���̕\��
            Cursor.lockState = CursorLockMode.Confined;
        }

        // �v���C���[��HP��0�ɂȂ�����
        if (player.isDie)
        {
            // �K�v�ȃf�[�^�̎󂯓n��
            manager.AddBattleResult(1);
            sceneChenge.ChengeScene("ResultScene");

            // �J�[�\���̕\��
            Cursor.lockState = CursorLockMode.Confined;
        }

        // Escape�L�[����͂�����J�[�\���̕\��
        if(Input.GetKey(KeyCode.Escape)) Cursor.lockState = CursorLockMode.Confined;
    }

    /// <summary>
    /// �o�ߎ��Ԃ��Z�b�g
    /// </summary>
    void SetNowTime()
    {
        nowTime = Time.time - startTime;
    }

    /// <summary>
    /// UI�𓮂���
    /// </summary>
    void UIManagement()
    {
        uiManager.TimeTextUI(nowTime);
        uiManager.PlayerHPUI((float)player.GetNowHP() / AllStatus.GetWeponHP(), (float)player.GetLateHP() / AllStatus.GetWeponHP());
        uiManager.MoveUI();
    }

    /// <summary>
    /// NavMesh�̍Đ���
    /// </summary>
    void ResetNavMesh()
    {
        mapNavMesh.BuildNavMesh();
    }
}

using AllGameManager;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Assets")]
    private AllGameStates status;
    [SerializeField] GroundChecker groundCheck;

    [Space, Header("Status")]
    [Range(1, 10), SerializeField, Tooltip("")] int attackDamage;
    [SerializeField] bool canRun = true;
    private int nowHP;
    private int lateHP;
    private int stanPoint;
    public bool isStan { get; private set; } = false;
    public bool isDie  { get; private set; } = false;
    public bool isAttack { get; set; }

    // Key input
    /// <summary> Flont key input </summary>
    public bool flont { get; private set; }
    /// <summary> Back key input </summary>
    public bool back { get; private set; }
    /// <summary> Right key input </summary>
    public bool right { get; private set; }
    /// <summary> Left key input </summary>
    public bool left { get; private set; }
    /// <summary> Jump key input </summary>
    public bool jump { get; private set; }
    /// <summary> Run key input </summary>
    public bool run { get; private set; }
    /// <summary> Attack key input </summary>
    public bool attack { get; private set; }
    /// <summary> Unique key input </summary>
    public bool unique { get; private set; }

    public void AddHP(int damage)       { nowHP -= damage; }
    public void AddStanPoint(int point) { stanPoint += point; }
    public void AddLateHP()             { lateHP = nowHP; }

    public int GetNowHP() { return nowHP; }
    public int GetLateHP() { return lateHP; }

    private void Awake()
    {
        status = GameObject.FindWithTag("GameManager").GetComponent<AllGameStates>();
        nowHP = status.GetPlayerHP();
    }

    private void FixedUpdate()
    {
        KeyInput();
        Die();
        Stan();
        FallDown();
    }
    void Die()
    {
        isDie = nowHP <= 0;
    }
    void Stan()
    {
        isStan = stanPoint >= 100;
        if (isStan) stanPoint = 0;
    }
    void FallDown()
    {
        if(transform.position.y < -100)
        {
            nowHP = 0;
        }
    }

    private void KeyInput()
    {
        flont = Input.GetKey(Controller.Flont);
        back = Input.GetKey(Controller.Back);
        right = Input.GetKey(Controller.Right);
        left = Input.GetKey(Controller.Left);
        jump = Input.GetKey(Controller.Jump) & groundCheck.isGrounded;
        run = Input.GetKey(Controller.Run) & canRun;
        attack = Input.GetKey(Controller.Attack);
        unique = Input.GetKey(Controller.Unique);
    }
}

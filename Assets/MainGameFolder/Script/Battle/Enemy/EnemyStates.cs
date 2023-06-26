using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    [SerializeField] BattleManagement _Manager;
    [SerializeField] BattleUIManagement _UIManager;
    [SerializeField] EnemyMove move;
    [Range(1000, 100000), SerializeField] int startHP;
    [Range(100, 20000), SerializeField] int startMaxStanPoint;
    [Range(0f, 1f), SerializeField] float _dot;
    [Range(20, 400), SerializeField] int attackDamage;

    //Private states
    private int nowHP;
    private int stanPoint;
    private int maxStanPoint;
    public bool isDie { get; private set; }
    public bool isStan { get; private set; }
    public bool isDamage { get; private set; }
    public bool isDieAnimEnd => isDie & move.isNullAnim;

    public void AddHP(int damage) { nowHP -= damage; }
    public void AddStanPoint(int point) { stanPoint += point; }

    public int GetNowHP() { return nowHP; }
    public int GetStanPoint() { return stanPoint; }
    public float GetDot() { return _dot; }
    public int GetAttackDamage() { return attackDamage; }

    public void DamageUI(Collider col, int damage)
    {
        _UIManager.DamageUI(col, damage);
        _Manager.AddAllDamage(damage);
    }

    private void Awake()
    {
        _Manager = GameObject.Find("BattleManager").GetComponent<BattleManagement>();
        _UIManager = GameObject.Find("BattleManager").GetComponent<BattleUIManagement>();
        nowHP = startHP;
        maxStanPoint = startMaxStanPoint;
        isDamage = false;
        isDie = false;
    }

    private void FixedUpdate()
    {
        Damage();
        Die();
        Stan();
    }
    void Damage()
    {
        isDamage = startHP > nowHP;
    }
    void Die()
    {
        isDie = nowHP <= 0;
    }
    void Stan()
    {
        if (isStan) { stanPoint = 0; maxStanPoint = (int)(maxStanPoint * 1.5); }
        isStan = stanPoint >= maxStanPoint;
    }
}

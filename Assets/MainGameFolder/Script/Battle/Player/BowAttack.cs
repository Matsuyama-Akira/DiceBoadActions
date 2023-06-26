using UnityEngine;
using AllGameManager;

public class BowAttack : MonoBehaviour
{
    //Private states.
    public enum BowType { Noumal, Penetrate, Diffusion,}
    public BowType type     { get; private set; }
    public float resetShot  { get; private set; }
    public float chargeTime { get; private set; }
    public bool isShoot     { get; private set; }
    public bool isCharge    { get; private set; }
    private bool isCharging;
    public bool isWepon     { get; set; } = true;

    [Header("GetAsset")]
    public BowParamTable getParam;
    private WeponSellect sellect;
    private AllGameSEManager seManager;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowPoint;
    [SerializeField] BattleUIManagement battleUI;
    [SerializeField] ChargeMaterUI chargeUI;
    [SerializeField] PlayerStatus status;
    [Space]
    [Header("States")]
    [Range(1f, 1000f)][SerializeField] private float arrowSpeedMagunification;
    [Range(0.1f, 10.0f)][SerializeField] private float shotInterval;
    [Range(2.0f, 10.0f)][SerializeField] private float maxCharge;
    private float nurfDamage = 5f;
    private int baseDamage;
    private float magunification;
    private int criticalRange;

    void Awake()
    {
        GameObject manager = GameObject.FindWithTag("GameManager");
        GameObject Bmanager = GameObject.Find("BattleManager");
        sellect = manager.GetComponent<WeponSellect>();
        seManager = manager.GetComponent<AllGameSEManager>();
        battleUI = Bmanager.GetComponent<BattleUIManagement>();
        chargeUI = Bmanager.GetComponent<ChargeMaterUI>();
        BowSetUP();
    }
    void BowSetUP()
    {
        baseDamage     = getParam.BowList[(int)sellect.rarelity].baseDamage;
        magunification = getParam.BowList[(int)sellect.rarelity].magunification;
        criticalRange  = getParam.BowList[(int)sellect.rarelity].criticalRange;
        BowTypeSet();
    }
    void BowTypeSet()
    {
        switch (getParam.BowList[(int)sellect.rarelity].type)
        {
            case BowSellection.BowType.Noumal:    type = BowType.Noumal;    break;
            case BowSellection.BowType.Penetrate: type = BowType.Penetrate; break;
            case BowSellection.BowType.Diffusion: type = BowType.Diffusion; break;
        }
    }

    void Update()
    {
        if (isWepon)
        {
            Attack();
            Interval();
        }
    }
    void Attack()
    {
        isShoot = isCharge & !status.attack;
        isCharge = resetShot <= 0 & status.attack;
        if (isCharge) Charge();
        if (isShoot)
        {
            ShootType();
            Resetter();
        }
    }
    void Charge()
    {
        if (chargeTime == 0f & seManager.BT_BowAttackSE[0] != null) seManager.BT_GameSESource.PlayOneShot(seManager.BT_BowAttackSE[0]);
        chargeTime += maxCharge * Time.deltaTime;
        if(chargeTime >= maxCharge) { chargeTime = maxCharge; }
        chargeUI.StartMeterRadial();
    }
    void ShootType()
    {
        switch (type)
        {
            case BowType.Noumal:
                int noumalRoop = 4;
                while (noumalRoop > 0)
                {
                    Shoot(0);
                    noumalRoop--;
                }
                break;
            case BowType.Penetrate:
                Shoot(1); break;
            case BowType.Diffusion:
                int difRoop = 5;
                while (difRoop > 0)
                {
                    Shoot(0);
                    difRoop--;
                }
                break;
        }
    }
    void Shoot(int num)
    {
        if(seManager.BT_BowAttackSE[1] != null) seManager.BT_GameSESource.PlayOneShot(seManager.BT_BowAttackSE[1]);
        int _ADamage = (int)(baseDamage * magunification * chargeTime / nurfDamage);
        GameObject _A = Instantiate(arrow, new Vector3(arrowPoint.position.x, arrowPoint.position.y, arrowPoint.position.z), arrowPoint.rotation);
        Rigidbody arrowRB = _A.GetComponent<Rigidbody>();
        _A.transform.forward = arrowPoint.forward;
        arrowRB.AddForce(_A.transform.forward * chargeTime * arrowSpeedMagunification * 0.1f);
        if(chargeTime == maxCharge) { _ADamage = (int)(_ADamage * 1.5); }
        _A.GetComponent<ArrowHit>().SetDamage(_ADamage);
        _A.GetComponent<ArrowHit>().SetCriticalRange(criticalRange);
        _A.GetComponent<ArrowHit>().SetArrowType(num);
        Destroy(_A, 20.0f);
    }
    void Resetter()
    {
        resetShot = shotInterval;
        chargeTime = 0f;
    }
    void Interval()
    {
        if (resetShot > 0)
        {
            resetShot -= 0.1f * Time.deltaTime;
            chargeUI.ReroadingUI(resetShot / shotInterval);
        }
    }

    public float MathCharge() { return chargeTime / maxCharge; }
}

using UnityEngine;
using AllGameManager;

public class BowAttack : MonoBehaviour
{
    //Private states.
    /// <summary> 弓の攻撃パターン </summary>
    public enum BowType { Noumal, Penetrate, Diffusion, }
    /// <summary> 攻撃パターンの選択 </summary>
    public BowType type     { get; private set; }
    /// <summary> 再度打てるようになるまでのゲージ。攻撃すると0になる </summary>
    public float resetShot  { get; private set; }
    /// <summary> チャージし始めてからの経過時間 </summary>
    public float chargeTime { get; private set; }
    /// <summary> 矢を放ったか </summary>
    public bool isShoot     { get; private set; }
    /// <summary> チャージしているか </summary>
    public bool isCharging    { get; private set; }
    /// <summary> 攻撃可能か </summary>
    public bool isAttackable     { get; set; } = true;

    [Header("GetAsset")]
    // 必須アセット
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
    [Range(1f, 1000f), SerializeField, Tooltip("放った矢の移動速度")] private float arrowSpeedMagunification;
    [Range(0.1f, 10.0f), SerializeField, Tooltip("リチャージのインターバル")] private float shotInterval;
    [Range(2.0f, 10.0f), SerializeField, Tooltip("チャージの最大値")] private float maxCharge;
    [Range(0.01f, 5.00f), SerializeField, Tooltip("弱体化倍率")] private float nurfDamage = 0.2f;
    /// <summary> 基本ダメージ </summary>
    private int baseDamage;
    /// <summary> チャージ量による倍率 </summary>
    private float magunification;
    /// <summary> クリティカル確率 </summary>
    private int criticalRange;

    /// <summary>
    /// 各種セットアップ
    /// </summary>
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

    /// <summary>
    /// レアリティからパラメーターを取得
    /// </summary>
    void BowSetUP()
    {
        baseDamage     = getParam.BowList[(int)sellect.rarelity].baseDamage;
        magunification = getParam.BowList[(int)sellect.rarelity].magunification;
        criticalRange  = getParam.BowList[(int)sellect.rarelity].criticalRange;
        BowTypeSet();
    }
    /// <summary>
    /// レアリティから攻撃パターンを取得
    /// </summary>
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
        // 攻撃可能であれば攻撃のための処理を行う
        if (isAttackable)
        {
            Attack();
            Interval();
        }
        // 攻撃不可の場合、ステータスのリセットだけ処理する
        else ResetByNonAttack();
    }
    /// <summary>
    /// 攻撃処理
    /// </summary>
    void Attack()
    {
        // チャージ中に攻撃キーを離すと矢を放つ
        isShoot = isCharging & !status.attack;
        if (isShoot)
        {
            Shoot();
            Resetter();
        }

        // 矢を放てる状態で攻撃キーを入力するとチャージを開始する
        isCharging = resetShot >= shotInterval & status.attack;
        if (isCharging) Charge();
    }
    /// <summary>
    /// チャージの処理
    /// </summary>
    void Charge()
    {
        // チャージ音を鳴らす
        if (chargeTime == 0f & seManager.BT_BowAttackSE[0] != null) seManager.BT_GameSESource.PlayOneShot(seManager.BT_BowAttackSE[0]);

        // チャージがマックスになるまでチャージする
        chargeTime += maxCharge * Time.deltaTime;
        if(chargeTime >= maxCharge) chargeTime = maxCharge;

        // チャージ用のUIを起動する
        chargeUI.StartMeterRadial();
    }
    /// <summary>
    /// 攻撃パターン毎に射撃方法を変える
    /// </summary>
    void Shoot()
    {
        // 矢を放ったSEを鳴らす
        if (seManager.BT_BowAttackSE[1] != null) seManager.BT_GameSESource.PlayOneShot(seManager.BT_BowAttackSE[1]);

        // 攻撃パターン毎に矢の生成方法を変える
        switch (type)
        {
            case BowType.Noumal:
                NoumalShoot();
                break;
            case BowType.Penetrate:
                PenetrationShoot();
                break;
            case BowType.Diffusion:
                DiffusionShoot();
                break;
        }
    }
    /// <summary>
    /// 攻撃パターンが通常射撃の処理
    /// </summary>
    void NoumalShoot()
    {
        // 矢の生成を4回行う
        int Roop = 4;
        while (Roop > 0)
        {
            // ダメージ量の計算
            int _ADamage = (int)(baseDamage * magunification * chargeTime * nurfDamage);

            // 矢の生成
            GameObject _A = Instantiate(arrow, new Vector3(arrowPoint.position.x, arrowPoint.position.y, arrowPoint.position.z), arrowPoint.rotation * Quaternion.AngleAxis(Random.Range(-10.0f, 10.0f), Vector3.up));
            Rigidbody arrowRB = _A.GetComponent<Rigidbody>();

            // チャージ量による処理
            arrowRB.AddForce(_A.transform.forward * chargeTime * arrowSpeedMagunification * 0.1f);
            if (chargeTime == maxCharge) { _ADamage = (int)(_ADamage * 1.5); }

            // 矢のパラメーターをセット
            _A.GetComponent<ArrowHit>().SetDamage(_ADamage);
            _A.GetComponent<ArrowHit>().SetCriticalRange(criticalRange);
            _A.GetComponent<ArrowHit>().SetArrowType(0);

            // 20秒後に消滅
            Destroy(_A, 20.0f);

            // ループ回数を減少
            Roop--;
        }
    }
    /// <summary>
    /// 攻撃パターンが貫通射撃の場合
    /// </summary>
    void PenetrationShoot()
    {
        int _ADamage = (int)(baseDamage * magunification * chargeTime * nurfDamage);
        GameObject _A = Instantiate(arrow, new Vector3(arrowPoint.position.x, arrowPoint.position.y, arrowPoint.position.z), arrowPoint.rotation);
        Rigidbody arrowRB = _A.GetComponent<Rigidbody>();
        arrowRB.AddForce(_A.transform.forward * chargeTime * arrowSpeedMagunification * 0.1f);
        if (chargeTime == maxCharge) { _ADamage = (int)(_ADamage * 1.5); }
        _A.GetComponent<ArrowHit>().SetDamage(_ADamage);
        _A.GetComponent<ArrowHit>().SetCriticalRange(criticalRange);
        _A.GetComponent<ArrowHit>().SetArrowType(1);
        Destroy(_A, 20.0f);
    }
    /// <summary>
    /// 攻撃パターンが拡散射撃の場合
    /// </summary>
    void DiffusionShoot()
    {
        int Roop = 5;
        while (Roop > 0)
        {
            int _ADamage = (int)(baseDamage * magunification * chargeTime * nurfDamage);
            float addYRotation = 10 * (Roop - 1) - 20;
            Quaternion addRotation = Quaternion.Euler(0f, addYRotation, 0f);
            GameObject _A = Instantiate(arrow, new Vector3(arrowPoint.position.x, arrowPoint.position.y, arrowPoint.position.z), arrowPoint.rotation * addRotation);
            Rigidbody arrowRB = _A.GetComponent<Rigidbody>();
            arrowRB.AddForce(_A.transform.forward * chargeTime * arrowSpeedMagunification * 0.1f);
            if (chargeTime == maxCharge) { _ADamage = (int)(_ADamage * 1.5); }
            _A.GetComponent<ArrowHit>().SetDamage(_ADamage);
            _A.GetComponent<ArrowHit>().SetCriticalRange(criticalRange);
            _A.GetComponent<ArrowHit>().SetArrowType(0);
            Destroy(_A, 20.0f);
            Roop--;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void Resetter()
    {
        resetShot = 0;
        chargeTime = 0f;
    }
    /// <summary>
    /// 
    /// </summary>
    void Interval()
    {
        if (resetShot < shotInterval)
        {
            resetShot += 0.1f * Time.deltaTime;
        }
        else resetShot = shotInterval;
        chargeUI.ReroadingUI(resetShot / shotInterval);
    }

    /// <summary>
    /// 
    /// </summary>
    void ResetByNonAttack()
    {
        resetShot = shotInterval;
        chargeTime = 0f;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float MathCharge() { return chargeTime / maxCharge; }
}

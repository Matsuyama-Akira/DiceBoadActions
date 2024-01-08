using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    // 必須スクリプト
    [SerializeField] BattleManagement _Manager;
    [SerializeField] BattleUIManagement _UIManager;
    [SerializeField] EnemyMove move;

    // 調整可能ステータス
    [Range(1000, 100000), SerializeField, Tooltip("スタート時のHP")] int startHP;
    [Range(100, 20000), SerializeField, Tooltip("最初のスタンに必要な値")] int startMaxStanPoint;
    [Range(0f, 1f), SerializeField, Tooltip("視野角")] float _dot;
    [Range(20, 400), SerializeField, Tooltip("攻撃力")] int attackDamage;

    // 非表示ステータス
    /// <summary> 現在のHP </summary>
    private int nowHP;
    /// <summary> 現在のスタン値 </summary>
    private int stanPoint;
    /// <summary> 現在のスタンに必要な値 </summary>
    private int maxStanPoint;

    /// <summary> 死亡判定 </summary>
    public bool isDie { get; private set; }
    /// <summary> スタン判定 </summary>
    public bool isStan { get; private set; }
    /// <summary> ダメージ判定 </summary>
    public bool isDamage { get; private set; }
    /// <summary> 死亡アニメーション終了判定 </summary>
    public bool isDieAnimEnd => isDie & move.isNullAnim;

    // 数値の加減算
    /// <summary> 受けたダメージ分HPを減らす </summary>
    public void AddHP(int damage) { nowHP -= damage; }
    /// <summary> スタン値を増やす </summary>
    public void AddStanPoint(int point) { stanPoint += point; }

    // ステータスの取得
    /// <summary> 現在のHPを取得 </summary>
    public int GetNowHP() { return nowHP; }
    /// <summary> 現在のスタン値を取得 </summary>
    public int GetStanPoint() { return stanPoint; }
    /// <summary> 視野角を取得 </summary>
    public float GetDot() { return _dot; }
    /// <summary> 攻撃力を取得 </summary>
    public int GetAttackDamage() { return attackDamage; }

    /// <summary>
    /// ダメージ量の表示
    /// </summary>
    /// <param name="col"></param>
    /// <param name="damage"></param>
    public void DamageUI(Collider col, int damage)
    {
        // マネージャーへ変数の受け渡し
        _UIManager.DamageUI(col, damage);
        _Manager.AddAllDamage(damage);
    }

    private void Awake()
    {
        // セットアップ
        _Manager = GameObject.Find("BattleManager").GetComponent<BattleManagement>();
        _UIManager = GameObject.Find("BattleManager").GetComponent<BattleUIManagement>();

        // ステータスの初期化
        nowHP = startHP;
        maxStanPoint = startMaxStanPoint;
        isDamage = false;
        isDie = false;
    }

    private void FixedUpdate()
    {
        // ダメージを受けたかの判定
        isDamage = startHP > nowHP;

        // HPが無くなったかの判定
        isDie = nowHP <= 0;

        // スタン値が必要スタン値を超えたかの判定
        if (isStan) { stanPoint = 0; maxStanPoint = (int)(maxStanPoint * 1.5); }
        isStan = stanPoint >= maxStanPoint;
    }
}

using UnityEngine;

public class ArrowHit : MonoBehaviour
{
    /// <summary> 矢の属性 </summary>
    public enum ArrowType
    {
        Nomal = 0,
        Penetrate = 1,
    }

    [SerializeField, Tooltip("このオブジェクトのRigidbodyを入れる")] Rigidbody rb;
    [SerializeField, Tooltip("このオブジェクトの子のコライダーを入れる")] Collider col;
    [SerializeField, Tooltip("このオブジェクトの子のオーディオソースを入れる")] AudioSource HitSESource;

    /// <summary> 矢の属性データ </summary>
    private ArrowType arrowType;
    /// <summary> 基本ダメージ </summary>
    private int arrowDamage;
    /// <summary> クリティカル確率 </summary>
    private int criticalRange;
    /// <summary> ヒット数 </summary>
    private int hitCount;

    /// <summary> SEのマネージャー </summary>
    private AllGameManager.AllGameSEManager seManager;

    /// <summary>
    /// 各種セットアップ
    /// </summary>
    private void Awake()
    {
        seManager = GameObject.FindWithTag("GameManager").GetComponent<AllGameManager.AllGameSEManager>();
    }

    /// <summary>
    /// 矢の属性の選択
    /// </summary>
    /// <param name="type"> 対応する属性のパターン </param>
    public void SetArrowType(int type)
    {
        // 受け取った属性値を元に属性を変更する。0の場合は非貫通属性、1の場合は貫通属性
        switch (type)
        {
            case 0: arrowType = ArrowType.Nomal; break;
            case 1: arrowType = ArrowType.Penetrate; break;
        }
    }

    /// <summary> 基本ダメージのセット </summary>
    /// <param name="damage"> ダメージ量 </param>
    public void SetDamage(int damage)       { arrowDamage = damage; }
    /// <summary> クリティカル確率のセット </summary>
    /// <param name="range"> クリティカル確率 </param>
    public void SetCriticalRange(int range) { criticalRange = range; }
    /// <summary> ヒット数の増加 </summary>
    /// <param name="hit"> 追加ヒット数(基本は1) </param>
    public void AddHitCount(int hit)        { hitCount += hit; }

    /// <summary> 矢の属性を返す </summary>
    /// <returns> 矢の属性(貫通/非貫通) </returns>
    public ArrowType GetArrowType() { return arrowType; }
    /// <summary> 基本ダメージ量を返す </summary>
    /// <returns> 基本ダメージ </returns>
    public int GetDamage()          { return arrowDamage; }
    /// <summary> クリティカル確率を返す </summary>
    /// <returns> クリティカル確率 </returns>
    public int GetCriticalRange()   { return criticalRange; }
    /// <summary> 現在のヒット数を返す </summary>
    /// <returns> ヒット数 </returns>
    public int GetHitCount()        { return hitCount; }

    /// <summary>
    /// このオブジェクトのコライダーが衝突した時
    /// </summary>
    /// <param name="other"> 衝突対象のコライダー </param>
    private void OnTriggerEnter(Collider other)
    {
        // 衝突対象がマップなら
        if (other.gameObject.tag == "Map")
        {
            // ヒット音を鳴らす
            if(seManager.BT_BowAttackSE[2] != null) HitSESource.PlayOneShot(seManager.BT_BowAttackSE[2]);

            // 各種ステータスを0にして3秒後に消滅させる
            rb.isKinematic = true;
            rb.useGravity = false;
            arrowDamage = 0;
            col.enabled = false;
            Destroy(gameObject, 3.0f);
        }
        if(other.gameObject.tag == "Enemy")
        {
            // ヒット音を鳴らす
            if (seManager.BT_BowAttackSE[2] != null) seManager.BT_GameSESource.PlayOneShot(seManager.BT_BowAttackSE[2]);
        }
    }
}

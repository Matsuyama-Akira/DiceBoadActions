using UnityEngine;
using AllGameManager;

public class SwordAttack : MonoBehaviour
{
    [Header("GetAssets")]
    private WeponSellect sellect;
    private AllGameSEManager seManager;
    public SwoudParamTable getParam;
    [SerializeField] PlayerStatus status;
    [SerializeField] Collider attackCol;

    /// <summary> 基礎ダメージ </summary>
    private int baseDamage;
    /// <summary> クリティカル確率 </summary>
    private int criticalRange;

    void Start()
    {
        // セットアップ
        sellect = GameObject.FindWithTag("GameManager").GetComponent<WeponSellect>();
        seManager = GameObject.FindWithTag("GameManager").GetComponent<AllGameSEManager>();
        SwordSetUP();
    }
    private void SwordSetUP()
    {
        // レアリティからパラメーターを取得
        baseDamage = getParam.SwordList[(int)sellect.rarelity].baseDamage;
        criticalRange = getParam.SwordList[(int)sellect.rarelity].criticalRange;
    }

    /// <returns> 基礎ダメージ </returns>
    public int GetDamage() { return baseDamage; }
    /// <returns> クリティカル確率 </returns>
    public int GetCriticalRange() { return criticalRange; }

    /// <summary> 攻撃SEの再生 </summary>
    /// <param name="num"> 攻撃の種類 </param>
    public void AddAttack(int num) { if (seManager.BT_SwordAttackSE[num] != null)
            seManager.BT_GameSESource.PlayOneShot(seManager.BT_SwordAttackSE[num]); }
    /// <summary> ガードSEの再生 </summary>
    public void AddGuard() { if (seManager.BT_SwordAttackSE[2] != null)
            seManager.BT_GameSESource.PlayOneShot(seManager.BT_SwordAttackSE[2]); }
    /// <summary> 攻撃ヒットSEの再生 </summary>
    public void AddHit() { if(seManager.BT_SwordAttackSE[3] != null)
            seManager.BT_GameSESource.PlayOneShot(seManager.BT_SwordAttackSE[3]); }
}

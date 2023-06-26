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

    //private status
    private int baseDamage;
    private int criticalRange;

    void Start()
    {
        sellect = GameObject.FindWithTag("GameManager").GetComponent<WeponSellect>();
        seManager = GameObject.FindWithTag("GameManager").GetComponent<AllGameSEManager>();
        SwordSetUP();
    }
    private void SwordSetUP()
    {
        baseDamage = getParam.SwordList[(int)sellect.rarelity].baseDamage;
        criticalRange = getParam.SwordList[(int)sellect.rarelity].criticalRange;
    }

    public int GetDamage() { return baseDamage; }
    public int GetCriticalRange() { return criticalRange; }

    public void AddAttack(int num) { if (seManager.BT_SwordAttackSE[num] != null)
            seManager.BT_GameSESource.PlayOneShot(seManager.BT_SwordAttackSE[num]); }
    public void AddGuard() { if (seManager.BT_SwordAttackSE[2] != null)
            seManager.BT_GameSESource.PlayOneShot(seManager.BT_SwordAttackSE[2]); }
    public void AddHit() { if(seManager.BT_SwordAttackSE[3] != null)
            seManager.BT_GameSESource.PlayOneShot(seManager.BT_SwordAttackSE[3]); }
}

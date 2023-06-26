using UnityEngine;

public class ArrowHit : MonoBehaviour
{
    public enum ArrowType
    {
        Nomal = 0,
        Penetrate = 1,
    }

    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;
    [SerializeField] AudioSource HitSESource;
    private ArrowType arrowType;
    private int arrowDamage;
    private int criticalRange;
    private int hitCount;

    private AllGameManager.AllGameSEManager seManager;

    private void Awake()
    {
        seManager = GameObject.FindWithTag("GameManager").GetComponent<AllGameManager.AllGameSEManager>();
    }

    public void SetArrowType(int num)
    {
        switch (num)
        {
            case 0: arrowType = ArrowType.Nomal; break;
            case 1: arrowType = ArrowType.Penetrate; break;
        }
    }
    public void SetDamage(int damage)       { arrowDamage = damage; }
    public void SetCriticalRange(int range) { criticalRange = range; }
    public void AddHitCount(int hit)        { hitCount += hit; }

    public ArrowType GetArrowType() { return arrowType; }
    public int GetDamage()          { return arrowDamage; }
    public int GetCriticalRange()   { return criticalRange; }
    public int GetHitCount()        { return hitCount; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Map")
        {
            if(seManager.BT_BowAttackSE[2] != null) HitSESource.PlayOneShot(seManager.BT_BowAttackSE[2]);
            rb.isKinematic = true;
            rb.useGravity = false;
            arrowDamage = 0;
            col.enabled = false;
            Destroy(gameObject, 3.0f);
        }
        if(other.gameObject.tag == "Enemy")
        {
            if (seManager.BT_BowAttackSE[2] != null) seManager.BT_GameSESource.PlayOneShot(seManager.BT_BowAttackSE[2]);
        }
    }
}

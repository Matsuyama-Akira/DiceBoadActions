using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    [Range(0, 100)] [SerializeField] float partMagunification;
    public EnemyStates states => parent;
    private EnemyStates parent;

    private void Start()
    {
        parent = GetComponentInParent<EnemyStates>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "ArrowCollider")
        {
            ArrowHit hit = other.GetComponentInParent<ArrowHit>();
            int damage = (int)(hit.GetDamage() * (1 + partMagunification / 100));
            int critical = Random.Range(0, 100);
            if (critical <= hit.GetCriticalRange()) damage = (int)(damage * 1.2); 
            parent.AddHP(damage);
            parent.AddStanPoint((int)damage / 10);
            parent.DamageUI(other, damage);
            if (hit.GetArrowType() == ArrowHit.ArrowType.Nomal | hit.GetHitCount() >= 4) { Destroy(other.gameObject.transform.root.gameObject); Debug.Log("Destroy"); return; }
            hit.AddHitCount(1);
            Debug.Log(hit.GetHitCount());
            return;
        }
        if(other.gameObject.tag == "PlayerSwordCollider")
        {
            Debug.Log(other.gameObject.name);
            SwordAttack hit = other.GetComponentInParent<SwordAttack>();
            string playAnim = other.GetComponentInParent<PlayerController>().GetNowAnim();
            int damage = (int)(hit.GetDamage() * (1 + partMagunification / 100));
            int critical = Random.Range(0, 100);
            if (critical <= hit.GetCriticalRange()) damage = (int)(damage * 1.2);
            switch (playAnim)
            {
                case "Attack":
                    damage = (int)(damage * 1.5);
                    break;
                case "Attack2":
                    damage = (int)(damage * 1.8);
                    break;
            }
            parent.AddHP(damage);
            parent.AddStanPoint((int)damage / 10);
            parent.DamageUI(other, damage);
            hit.AddHit();
            return;
        }
    }
}

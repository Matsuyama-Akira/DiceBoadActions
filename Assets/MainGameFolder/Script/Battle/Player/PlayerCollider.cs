using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private PlayerStatus status;
    private PlayerController controller;

    private void Awake()
    {
        status = GetComponentInParent<PlayerStatus>();
        controller = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "EnemyAttackCollider")
        {
            if (other.GetComponentInParent<EnemyAttack>().GetHit() < 1)
            {
                status.AddLateHP();
                status.AddHP(other.GetComponentInParent<EnemyStates>().GetAttackDamage());
                other.GetComponentInParent<EnemyAttack>().AddHit(1);
                controller.AddHitAnim();
            }
        }
    }
}

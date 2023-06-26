using UnityEngine;

public class ReceiveEventForAnimation : MonoBehaviour
{
    [SerializeField] SwordAttack sword;

    public void AddAttack(int numAttack)
    {
        sword.AddAttack(numAttack);
    }

    public void AddGuard()
    {
        sword.AddGuard();
    }

    public void AddHit()
    {
        sword.AddHit();
    }
}

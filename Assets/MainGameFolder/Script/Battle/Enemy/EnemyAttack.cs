using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private int Hit;

    public void AddHit(int hit) { Hit += hit; }
    public void ResetHit() { Hit = 0; }
    public int GetHit() { return Hit; }
}

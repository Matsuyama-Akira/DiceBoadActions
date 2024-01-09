using UnityEngine;

public class ReceiveEventForAnimation : MonoBehaviour
{
    // 受け渡し先
    [SerializeField] SwordAttack sword;

    // 全てSE再生。アニメーションからメソッドを動かす
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

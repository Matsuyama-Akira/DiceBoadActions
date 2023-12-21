using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // ヒット数のカウント
    private int Hit;

    /// <summary> ヒット数の加算 </summary>
    public void AddHit(int hit) { Hit += hit; }
    /// <summary> ヒット数のリセット </summary>
    public void ResetHit() { Hit = 0; }
    /// <summary> ヒット数の送信 </summary>
    public int GetHit() { return Hit; }
}

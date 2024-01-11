using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    // 必須スクリプト
    private PlayerStatus status;
    private PlayerController controller;

    /// <summary>
    /// スクリプトのキャッシュ
    /// </summary>
    private void Awake()
    {
        status = GetComponentInParent<PlayerStatus>();
        controller = GetComponentInParent<PlayerController>();
    }

    /// <summary>
    /// このスクリプトが適用されているコライダーに衝突したときの処理
    /// </summary>
    /// <param name="other"> 対象のコライダー </param>
    private void OnTriggerEnter(Collider other)
    {
        // 衝突したコライダーのタグがEnemyAttackColliderならば
        if(other.gameObject.tag == "EnemyAttackCollider")
        {
            // EnemyAttackのヒット数が0ならばダメージ処理を行う
            if (other.GetComponentInParent<EnemyAttack>().GetHit() < 1)
            {
                // プレイヤーのダメージを受ける前のHPを代入
                status.AddLateHP();

                // 受けたダメージ分HPを減らす
                status.AddHP(other.GetComponentInParent<EnemyStates>().GetAttackDamage());

                // EnemyAttackのヒット数を1増やす
                other.GetComponentInParent<EnemyAttack>().AddHit(1);

                // プレイヤーのダメージアニメーションを再生
                controller.AddHitAnim();
            }
        }
    }
}

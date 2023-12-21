using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    [Range(0, 100), SerializeField, Tooltip("部位毎のダメージ倍率")] float partMagunification;
    /// <summary> このキャラクターのHP等のデータ </summary>
    private EnemyStates parent;

    private void Start()
    {
        parent = GetComponentInParent<EnemyStates>();
    }

    /// <summary>
    /// 衝突したコライダーによるダメージ処理
    /// </summary>
    /// <param name="other"> 対象の衝突したコライダー </param>
    private void OnTriggerEnter(Collider other)
    {
        // 対象がArrowオブジェクトの場合
        if(other.gameObject.name == "ArrowCollider")
        {
            // コライダーオブジェクトからダメージデータをロード
            ArrowHit hit = other.GetComponentInParent<ArrowHit>();

            // ダメージ計算
            int damage = (int)(hit.GetDamage() * (1 + partMagunification / 100));
            int critical = Random.Range(0, 100);
            if (critical <= hit.GetCriticalRange()) damage = (int)(damage * 1.2);

            // HP減少などの処理
            parent.AddHP(damage);
            parent.AddStanPoint((int)damage / 10);

            // ダメージ量のUIを表示
            parent.DamageUI(other, damage);

            // 対象のコライダーオブジェクトの消滅処理
            if (hit.GetArrowType() == ArrowHit.ArrowType.Nomal | hit.GetHitCount() >= 4) { Destroy(other.gameObject.transform.root.gameObject); Debug.Log("Destroy"); return; }

            // 対象のヒット数加算
            hit.AddHitCount(1);
            return;
        }

        // 対象がSwordオブジェクトの場合
        if(other.gameObject.tag == "PlayerSwordCollider")
        {
            // コライダーオブジェクトからダメージデータをロード
            SwordAttack hit = other.GetComponentInParent<SwordAttack>();

            // 対象のアニメーションデータを取得
            string playAnim = other.GetComponentInParent<PlayerController>().GetNowAnim();

            // ダメージ計算
            int damage = (int)(hit.GetDamage() * (1 + partMagunification / 100));
            int critical = Random.Range(0, 100);
            if (critical <= hit.GetCriticalRange()) damage = (int)(damage * 1.2);

            // アニメーション毎のダメージ倍率計算
            switch (playAnim)
            {
                case "Attack":
                    damage = (int)(damage * 1.5);
                    break;
                case "Attack2":
                    damage = (int)(damage * 1.8);
                    break;
            }

            // HP減少などの処理
            parent.AddHP(damage);
            parent.AddStanPoint((int)damage / 10);

            // ダメージ量のUIを表示
            parent.DamageUI(other, damage);

            // 対象のヒット数加算
            hit.AddHit();
            return;
        }
    }
}

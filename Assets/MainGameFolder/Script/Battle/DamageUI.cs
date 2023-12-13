using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageUI : MonoBehaviour
{
    /// <summary> プレイヤーのカメラのトランスフォーム </summary>
    private Transform playerCam;
    /// <summary> ダメージ量のテキスト </summary>
    private TextMeshProUGUI damageText;
    /// <summary> フェードアウトの速度 </summary>
    private float fadeOutSpeed = 1f;
    /// <summary> フェードアウト中の移動速度 </summary>
    [SerializeField]
    private float moveSpeed = 0.4f;

    /// <summary>
    /// セットアップ
    /// </summary>
    private void Awake()
    {
        // ワールドカメラにプレイヤーカメラを入れて画面に表示できるように
        GetComponent<Canvas>().worldCamera =
            GameObject.FindWithTag("Player").GetComponentInChildren<Camera>();

        // プレイヤーカメラをキャッシュ
        playerCam = Camera.main.transform;

        // テキストをキャッシュ
        damageText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        // UIの挙動
        transform.rotation = playerCam.rotation;
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // 徐々に透明に
        damageText.color = Color.Lerp(damageText.color, new Color(1f, 0f, 0f, 0f), fadeOutSpeed * Time.deltaTime);

        // テキストのアルベドが0.1を切ったらUIを消滅させる
        if (damageText.color.a <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
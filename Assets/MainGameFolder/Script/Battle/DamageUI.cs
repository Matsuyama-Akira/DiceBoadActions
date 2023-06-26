using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageUI : MonoBehaviour
{

    private TextMeshProUGUI damageText;
    //　フェードアウトするスピード
    private float fadeOutSpeed = 1f;
    //　移動値
    [SerializeField]
    private float moveSpeed = 0.4f;

    private void Awake()
    {
        GetComponent<Canvas>().worldCamera =
            GameObject.FindWithTag("Player").GetComponentInChildren<Camera>();
    }

    void Start()
    {
        damageText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        damageText.color = Color.Lerp(damageText.color, new Color(1f, 0f, 0f, 0f), fadeOutSpeed * Time.deltaTime);

        if (damageText.color.a <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
using UnityEngine;
using AllGameManager;
using TMPro;

public class WeponSellectManager : MonoBehaviour
{
    private WeponSellect wepon;
    [SerializeField] TextMeshProUGUI weponText;
    [SerializeField] Transform weponSpawnPoint;
    [SerializeField, NamedArray(new string[5] { "Sword", "Spire", "Bow", "Gun", "Magic" })]
    private GameObject[] WeponObject = new GameObject[5];

    void Start()
    {
        wepon = GameObject.FindWithTag("GameManager").GetComponent<WeponSellect>();
    }

    public void WeponSellecter(string SellectWepon)
    {
        wepon.weponSellecting(SellectWepon);
        weponText.text = SellectWepon;
    }
}

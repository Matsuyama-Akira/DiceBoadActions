using UnityEngine;
using AllGameManager;
using TMPro;

public class WeponSellectManager : MonoBehaviour
{
    private WeponSellect wepon;
    [SerializeField] TextMeshProUGUI weponText;
    [SerializeField] Transform weponSpawnPoint;
    [SerializeField, NamedArray(new string[5] { "Sword", "Spear", "Bow", "Gun", "Magic" })]
    private GameObject[] WeponObject = new GameObject[5];
    private GameObject _wepon;

    void Start()
    {
        wepon = GameObject.FindWithTag("GameManager").GetComponent<WeponSellect>();
    }

    public void WeponSellecter(string SellectWepon)
    {
        wepon.weponSellecting(SellectWepon);
        weponText.text = SellectWepon;
        switch (SellectWepon)
        {
            case "Sword": WeponDisplay(0); break;
            case "Spear": WeponDisplay(1); break;
            case "Bow": WeponDisplay(2); break;
            case "Gun": WeponDisplay(3); break;
            case "Magic": WeponDisplay(4); break;
        }
    }

    private void WeponDisplay(int sellectWepon)
    {
        Destroy(_wepon);
        _wepon = Instantiate(WeponObject[sellectWepon], weponSpawnPoint);
    }
}

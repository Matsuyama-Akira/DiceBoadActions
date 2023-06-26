using UnityEngine;

public class WeponSellecter : MonoBehaviour
{
    [SerializeField] WeponSellectManager manager;
    [SerializeField] AddButton NextSceneButton;
    private void Awake()
    {
        //manager = GameObject.Find("WeponSellectManager").GetComponent<WeponSellectManager>();
        NextSceneButton = GameObject.Find("NextSceneButton").GetComponent<AddButton>();
    }

    public void SellectWepon(string sellect)
    {
        manager.WeponSellecter(sellect);
        NextSceneButton.enabled = true;
    }
}

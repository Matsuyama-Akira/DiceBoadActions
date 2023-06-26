using AllGameManager;
using TMPro;
using UnityEngine;

public class DiceBoadUIManagement : MonoBehaviour
{
    [SerializeField] GameObject PlayerTurnUI;
    [SerializeField] TextMeshProUGUI WeponText;
    [SerializeField] TextMeshProUGUI MovePointText;
    [SerializeField] GameObject WeponSellectUI;
    [SerializeField] TextMeshProUGUI NewWeponText;
    [SerializeField] TextMeshProUGUI OldWeponText;
    private WeponSellect wepon;
    private DiceBoadManagement _manager;

    void Start()
    {
        wepon = GameObject.FindWithTag("GameManager").GetComponent<WeponSellect>();
        _manager = GetComponent<DiceBoadManagement>();
    }

    void Update()
    {
        UIText();
        WeponSellect();
    }

    void UIText()
    {
        WeponText.text = wepon.wepon.ToString();
        MovePointText.text = _manager.GetMovePoint().ToString();
    }

    void WeponSellect()
    {
        if (_manager.GetIsWeponSellect())
        {
            WeponSellectUI.SetActive(true);
            string weponName = WeponName(), rarelity = RarelityName();
            NewWeponText.text = "New wepon\n" + weponName + " : " + rarelity;
            OldWeponText.text = "Old wepon\n" + wepon.wepon.ToString() + " : " + wepon.rarelity.ToString();
        }
        else WeponSellectUI.SetActive(false);
    }

    string WeponName()
    {
        string Name;
        switch (_manager.GetNewWepons(0))
        {
            case 0: Name = "Sword"; break;
            case 1: Name = "Spire"; break;
            case 2: Name = "Bow"; break;
            case 3: Name = "Gun"; break;
            case 4: Name = "Magic"; break;
            default: Name = null; break;
        }
        return Name;
    }
    string RarelityName()
    {
        string Name;
        switch (_manager.GetNewWepons(1))
        {
            case 0: Name = "Common"; break;
            case 1: Name = "Rare"; break;
            case 2: Name = "Unique"; break;
            default: Name = null; break;
        }
        return Name;
    }
}

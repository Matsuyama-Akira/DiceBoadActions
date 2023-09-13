using AllGameManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceBoadUIManagement : MonoBehaviour
{
    [SerializeField] GameObject PlayerTurnUI;
    [SerializeField, NamedArray(new string[] { "Sword", "Bow" })] GameObject[] WeponImageObj;
    [SerializeField, NamedArray(new string[] { "Sword", "Bow" })] Image[] WeponImage;
    [SerializeField, NamedArray(new string[] { "Common", "Rare", "Unique" })] Color32[] WeponRarelity; //0 = Common 1 = Rare 2 = Unique
    [SerializeField] Slider PlayerNowHP;
    [SerializeField] Image PlayerLateHP;
    [SerializeField] TextMeshProUGUI MovePointText;
    [SerializeField] GameObject WeponSellectUI;
    [SerializeField] TextMeshProUGUI NewWeponText;
    [SerializeField] TextMeshProUGUI OldWeponText;
    [SerializeField] TextMeshProUGUI StandbyRollText;
    [SerializeField] AnimationCurve StandbyWaitTime;

    private AllGameStates _gameStatus;
    public float LatePlayerHPPersent;
    public float DamagePlayerHPPersent;
    private bool AddLate;
    private float StartTime;
    private float nowTime;
    private WeponSellect wepon;
    private DiceBoadManagement _manager;

    void Start()
    {
        wepon = GameObject.FindWithTag("GameManager").GetComponent<WeponSellect>();
        _gameStatus = GameObject.FindWithTag("GameManager").GetComponent<AllGameStates>();
        _manager = GetComponent<DiceBoadManagement>();
    }

    void Update()
    {
        UIText();
        UIImage();
        WeponSellect();
    }

    void UIText()
    {
        MovePointText.text = _manager.GetMovePoint().ToString();
        if (!_manager.GetIsStandby()) StandbyRollText.alpha = 0;
        else
        {
            StandbyRollText.alpha = StandbyWaitTime.Evaluate(Time.time);
        }
    }

    void UIImage()
    {
        NowWeponImage();
        NowPlayerHPUI((float)_gameStatus.GetPlayerHP() / _gameStatus.GetWeponHP(), (float)_gameStatus.GetLateHP() / _gameStatus.GetWeponHP());
    }

    void NowWeponImage()
    {
        switch (wepon.wepon)
        {
            case AllGameManager.WeponSellect.Wepon.Sword:
                WeponImageObj[0].SetActive(true);
                WeponImageObj[1].SetActive(false);
                break;
            case AllGameManager.WeponSellect.Wepon.Bow:
                WeponImageObj[0].SetActive(false);
                WeponImageObj[1].SetActive(true);
                break;
        }

        switch (wepon.rarelity)
        {
            case AllGameManager.WeponSellect.Rarelity.Common:
                WeponImage[0].color = WeponRarelity[0];
                WeponImage[1].color = WeponRarelity[0];
                break;
            case AllGameManager.WeponSellect.Rarelity.Rare:
                WeponImage[0].color = WeponRarelity[1];
                WeponImage[1].color = WeponRarelity[1];
                break;
            case AllGameManager.WeponSellect.Rarelity.Unique:
                WeponImage[0].color = WeponRarelity[2];
                WeponImage[1].color = WeponRarelity[2];
                break;
        }
    }

    void NowPlayerHPUI(float nowPlayerHPPersent, float _LatePlayerHPPersent)
    {
        if (LatePlayerHPPersent != _LatePlayerHPPersent)
        {
            if (LatePlayerHPPersent < _LatePlayerHPPersent) AddLate = true;
            else AddLate = false;
            LatePlayerHPPersent = _LatePlayerHPPersent;
            DamagePlayerHPPersent = _LatePlayerHPPersent;
            StartTime = Time.time;
        }
        nowTime = Time.time - StartTime;
        if (AddLate)
        {
            if (nowPlayerHPPersent > DamagePlayerHPPersent & 2 < nowTime) DamagePlayerHPPersent += 1 * Time.deltaTime;
        }
        else
        {
            if (nowPlayerHPPersent < DamagePlayerHPPersent & 2 < nowTime) DamagePlayerHPPersent -= 1 * Time.deltaTime;
        }
        PlayerNowHP.value = nowPlayerHPPersent;
        PlayerLateHP.fillAmount = DamagePlayerHPPersent;
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

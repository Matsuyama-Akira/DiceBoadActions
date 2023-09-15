using AllGameManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceBoadUIManagement : MonoBehaviour
{
    [SerializeField] GameObject PlayerTurnUI;
    [SerializeField, NamedArray(new string[] { "Sword", "Bow" })] Image[] nowWeponImage;
    [SerializeField] Image nowWeponRarelity;
    [SerializeField, NamedArray(new string[] { "Sword", "Bow" })] Image[] newWeponImage;
    [SerializeField] Image newWeponRarelity;
    [SerializeField, NamedArray(new string[] { "Sword", "Bow" })] Image[] oldWeponImage;
    [SerializeField] Image oldWeponRarelity;
    [SerializeField, NamedArray(new string[] { "Common", "Rare", "Unique" })] Color32[] WeponRarelity; //0 = Common 1 = Rare 2 = Unique
    [SerializeField] Slider PlayerNowHP;
    [SerializeField] Image PlayerLateHP;
    [SerializeField] TextMeshProUGUI MovePointText;
    [SerializeField] GameObject WeponSellectUI;
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
                nowWeponImage[0].enabled = true;
                nowWeponImage[1].enabled = false;
                break;
            case AllGameManager.WeponSellect.Wepon.Bow:
                nowWeponImage[0].enabled = false;
                nowWeponImage[1].enabled = true;
                break;
        }

        nowWeponRarelity.color = WeponRarelity[(int)wepon.rarelity];
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
            NewWeponImage();
            OldWeponImage();
        }
        else WeponSellectUI.SetActive(false);
    }
    void NewWeponImage()
    {
        switch (_manager.GetNewWepons(0))
        {
            case 0:
                newWeponImage[0].enabled = true;
                newWeponImage[1].enabled = false;
                break;
            case 2:
                newWeponImage[0].enabled = false;
                newWeponImage[1].enabled = true;
                break;
        }

        newWeponRarelity.color = WeponRarelity[_manager.GetNewWepons(1)];
    }
    void OldWeponImage()
    {
        switch (wepon.wepon)
        {
            case AllGameManager.WeponSellect.Wepon.Sword:
                oldWeponImage[0].enabled = true;
                oldWeponImage[1].enabled = false;
                break;
            case AllGameManager.WeponSellect.Wepon.Bow:
                oldWeponImage[0].enabled = false;
                oldWeponImage[1].enabled = true;
                break;
        }

        oldWeponRarelity.color = WeponRarelity[(int)wepon.rarelity];
    }
}

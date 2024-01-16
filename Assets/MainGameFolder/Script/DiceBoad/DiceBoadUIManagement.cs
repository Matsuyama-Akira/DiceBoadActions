using AllGameManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceBoadUIManagement : MonoBehaviour
{
    // 動かしたいUI
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

    // 必須スクリプト
    private AllGameStates gameStatus;
    private WeponSellect wepon;
    private DiceBoadManagement DBmanager;

    // 動かすために必要なパラメーター
    public float LatePlayerHPPersent;
    public float DamagePlayerHPPersent;
    private bool AddLate;
    private float StartTime;
    private float nowTime;

    void Start()
    {
        // 必須スクリプトをセットアップ
        wepon = GameObject.FindWithTag("GameManager").GetComponent<WeponSellect>();
        gameStatus = GameObject.FindWithTag("GameManager").GetComponent<AllGameStates>();
        DBmanager = GetComponent<DiceBoadManagement>();
    }

    void Update()
    {
        UIText();
        UIImage();
        WeponSellect();
    }

    void UIText()
    {
        // テキストを更新
        MovePointText.text = DBmanager.GetMovePoint().ToString();
        if (!DBmanager.GetIsStandby()) StandbyRollText.alpha = 0;
        else StandbyRollText.alpha = StandbyWaitTime.Evaluate(Time.time);
    }

    void UIImage()
    {
        NowWeponImage();
        NowPlayerHPUI((float)gameStatus.GetPlayerHP() / gameStatus.GetWeponHP(), (float)gameStatus.GetLateHP() / gameStatus.GetWeponHP());
    }

    void NowWeponImage()
    {
        // 武器の種類から画像を変更
        switch (wepon.wepon)
        {
            case AllGameManager.WeponSellect.Wepon.Sword:
                nowWeponImage[0].enabled = false;
                nowWeponImage[1].enabled = true;
                break;
            case AllGameManager.WeponSellect.Wepon.Bow:
                nowWeponImage[0].enabled = true;
                nowWeponImage[1].enabled = false;
                break;
        }

        // 武器のレアリティから背景色を変更
        nowWeponRarelity.color = WeponRarelity[(int)wepon.rarelity];
    }

    /// <param name="nowPlayerHPPersent"> 現在のHPの割合 </param>
    /// <param name="_LatePlayerHPPersent"> HPが変化する前のHPの割合 </param>
    void NowPlayerHPUI(float nowPlayerHPPersent, float _LatePlayerHPPersent)
    {
        // HPが変化したら
        if (LatePlayerHPPersent != _LatePlayerHPPersent)
        {
            // HP状況を更新し、ダメージや回復のアニメーション処理をスタート
            if (LatePlayerHPPersent < _LatePlayerHPPersent) AddLate = true;
            else AddLate = false;
            LatePlayerHPPersent = _LatePlayerHPPersent;
            DamagePlayerHPPersent = _LatePlayerHPPersent;
            StartTime = Time.time;
        }

        // 経過時間
        nowTime = Time.time - StartTime;

        // ダメージまたは回復のアニメーション処理
        if (AddLate) if (nowPlayerHPPersent > DamagePlayerHPPersent & 2 < nowTime) DamagePlayerHPPersent += 1 * Time.deltaTime;
        else if (nowPlayerHPPersent < DamagePlayerHPPersent & 2 < nowTime) DamagePlayerHPPersent -= 1 * Time.deltaTime;
        PlayerNowHP.value = nowPlayerHPPersent;
        PlayerLateHP.fillAmount = DamagePlayerHPPersent;
    }

    void WeponSellect()
    {
        // 武器の選択処理が開始したらUIを表示
        if (DBmanager.GetIsWeponSellect())
        {
            WeponSellectUI.SetActive(true);
            NewWeponImage();
            OldWeponImage();
        }
        // 処理が終わったらUIを非表示
        else WeponSellectUI.SetActive(false);
    }
    void NewWeponImage()
    {
        // 新しい武器の種類から画像を更新
        switch (DBmanager.GetNewWepons(0))
        {
            case 0:
                newWeponImage[0].enabled = false;
                newWeponImage[1].enabled = true;
                break;
            case 2:
                newWeponImage[0].enabled = true;
                newWeponImage[1].enabled = false;
                break;
        }

        // 新しい武器のレアリティから背景色を更新
        newWeponRarelity.color = WeponRarelity[DBmanager.GetNewWepons(1)];
    }
    void OldWeponImage()
    {
        // 現在の武器の種類から画像を更新
        switch (wepon.wepon)
        {
            case AllGameManager.WeponSellect.Wepon.Sword:
                oldWeponImage[0].enabled = false;
                oldWeponImage[1].enabled = true;
                break;
            case AllGameManager.WeponSellect.Wepon.Bow:
                oldWeponImage[0].enabled = true;
                oldWeponImage[1].enabled = false;
                break;
        }

        // 現在の武器のレアリティから背景色を更新
        oldWeponRarelity.color = WeponRarelity[(int)wepon.rarelity];
    }
}

using UnityEngine;
using UnityEngine.UI;
using AllGameManager;
using TMPro;

public class BattleUIManagement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Slider PlayerNowHP;
    [SerializeField] Image PlayerLateHP;
    [SerializeField] GameObject damageUI;
    [NamedArray(new string[] {"Flont", "Back", "Left", "Right"}), SerializeField]
    Image[] MoveKeyUI = new Image[4];
    [NamedArray(new string[] { "Flont", "Back", "Left", "Right" }), SerializeField]
    TextMeshProUGUI[] MoveKeyText = new TextMeshProUGUI[4];

    private float LatePlayerHPPersent;
    private float DamagePlayerHPPersent;
    private float StartTime;
    private float nowTime;

    public void DamageUI(Collider col, int damage)
    {
        GameObject _damageUI = Instantiate(damageUI, col.bounds.center - Camera.main.transform.forward * 0.2f, Quaternion.identity);
        _damageUI.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
    }

    public void TimeTextUI(float time)
    {
        timeText.text = time.ToString();
    }

    public void PlayerHPUI(float nowPlayerHPPersent, float _LatePlayerHPPersent)
    {
        if (LatePlayerHPPersent != _LatePlayerHPPersent)
        {
            LatePlayerHPPersent = _LatePlayerHPPersent;
            DamagePlayerHPPersent = _LatePlayerHPPersent;
            StartTime = Time.time;
        }
        nowTime = Time.time - StartTime;
        if (nowPlayerHPPersent < DamagePlayerHPPersent & 2 < nowTime) DamagePlayerHPPersent -= 1 * Time.deltaTime;
        PlayerNowHP.value = nowPlayerHPPersent;
        PlayerLateHP.fillAmount = DamagePlayerHPPersent;
    }

    public void MoveUI(bool flont, bool back, bool left, bool right)
    {
        if (flont) MoveKeyUI[0].color = Color.red;
        else MoveKeyUI[0].color = Color.black;
        if (back) MoveKeyUI[1].color = Color.red;
        else MoveKeyUI[1].color = Color.black;
        if (left) MoveKeyUI[2].color = Color.red;
        else MoveKeyUI[2].color = Color.black;
        if (right) MoveKeyUI[3].color = Color.red;
        else MoveKeyUI[3].color = Color.black;

        MoveKeyText[0].text = Controller.Flont.ToString();
        MoveKeyText[1].text = Controller.Back.ToString();
        MoveKeyText[2].text = Controller.Left.ToString();
        MoveKeyText[3].text = Controller.Right.ToString();
    }
}

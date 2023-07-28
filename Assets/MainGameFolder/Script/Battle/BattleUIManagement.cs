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
    [NamedArray(new string[] { "Flont", "Back", "Left", "Right", "Sprint" }), SerializeField]
    Image[] KeyUI;
    [NamedArray(new string[] { "Flont", "Back", "Left", "Right", "Sprint" }), SerializeField]
    TextMeshProUGUI[] KeyText;

    private PlayerStatus _player;
    private float LatePlayerHPPersent;
    private float DamagePlayerHPPersent;
    private float StartTime;
    private float nowTime;

    public void SetStatus(PlayerStatus player) { _player = player; }

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

    public void MoveUI()
    {
        if (_player.flont) KeyUI[0].color = Color.red; else KeyUI[0].color = Color.black;
        if (_player.back)  KeyUI[1].color = Color.red; else KeyUI[1].color = Color.black;
        if (_player.left)  KeyUI[2].color = Color.red; else KeyUI[2].color = Color.black;
        if (_player.right) KeyUI[3].color = Color.red; else KeyUI[3].color = Color.black;
        if (_player.run)   KeyUI[4].color = Color.red; else KeyUI[4].color = Color.black;

        KeyText[0].text = Controller.Flont.ToString();
        KeyText[1].text = Controller.Back.ToString();
        KeyText[2].text = Controller.Left.ToString();
        KeyText[3].text = Controller.Right.ToString();
        KeyText[4].text = Controller.Run.ToString();
    }
}

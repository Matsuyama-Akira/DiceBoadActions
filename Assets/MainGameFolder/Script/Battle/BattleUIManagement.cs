using UnityEngine;
using UnityEngine.UI;
using AllGameManager;
using TMPro;

public class BattleUIManagement : MonoBehaviour
{
    // UIアセット
    /// <summary> 経過時間 </summary>
    [SerializeField] TextMeshProUGUI timeText;
    /// <summary> プレイヤーの現在のHP </summary>
    [SerializeField] Slider PlayerNowHP;
    /// <summary> プレイヤーの徐々に減るHP表現 </summary>
    [SerializeField] Image PlayerLateHP;
    /// <summary> 敵のダメージ表記 </summary>
    [SerializeField] GameObject damageUI;
    /// <summary> キー入力の表示 </summary>
    [NamedArray(new string[] { "Flont", "Back", "Left", "Right", "Sprint" }), SerializeField]
    Image[] KeyUI;
    /// <summary> キーの名前 </summary>
    [NamedArray(new string[] { "Flont", "Back", "Left", "Right", "Sprint" }), SerializeField]
    TextMeshProUGUI[] KeyText;

    // UIに必要なデータ
    /// <summary> プレイヤーのキー入力 </summary>
    private PlayerStatus _player;
    /// <summary> プレイヤーのダメージを受ける前のHPのパーセント </summary>
    private float LatePlayerHPPersent;
    /// <summary> ダメージを受けてから徐々に減るHPのパーセント </summary>
    private float DamagePlayerHPPersent;
    /// <summary> プレイヤーがダメージを受けたタイミングを0とした時間 </summary>
    private float StartTime;
    /// <summary> プレイヤーがダメージを受けてから経過した時間 </summary>
    private float nowTime;

    /// <summary>
    /// プレイヤーのステータスをキャッシュする
    /// </summary>
    /// <param name="player"> プレイヤーステータス </param>
    public void SetStatus(PlayerStatus player) { _player = player; }

    /// <summary>
    /// 敵のダメージ量を表示する
    /// </summary>
    /// <param name="col"> ダメージを受けた場所 </param>
    /// <param name="damage"> ダメージ量 </param>
    public void DamageUI(Collider col, int damage)
    {
        // 敵がダメージを受けた場所にダメージ量の表記UIを生成
        GameObject _damageUI = Instantiate(damageUI, col.bounds.center - Camera.main.transform.forward * 0.2f, Quaternion.identity);

        // 生成したUIのテキストをダメージ量に変更
        _damageUI.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
    }

    /// <summary>
    /// 現在の経過時間を表示
    /// </summary>
    /// <param name="time"> 現在の経過時間 </param>
    public void TimeTextUI(float time) { timeText.text = time.ToString(); }

    /// <summary>
    /// プレイヤーのHPのUI
    /// </summary>
    /// <param name="nowPlayerHPPersent"> 現在のプレイヤーのHP </param>
    /// <param name="_LatePlayerHPPersent"> プレイヤーのダメージを受ける前のHP </param>
    public void PlayerHPUI(float nowPlayerHPPersent, float _LatePlayerHPPersent)
    {
        // ダメージを受ける前のHPが変化したか
        if (LatePlayerHPPersent != _LatePlayerHPPersent)
        {
            // ダメージを受ける前のHPを更新
            LatePlayerHPPersent = _LatePlayerHPPersent;

            // ダメージを受ける前のHPを代入し、現在時間を0とする
            DamagePlayerHPPersent = _LatePlayerHPPersent;
            StartTime = Time.time;
        }

        // ダメージを受けてからの経過時間
        nowTime = Time.time - StartTime;

        // 現在のHPより徐々に減らすHPが多くて経過時間が2秒経っていれば、ダメージを受ける前のHPを徐々に減らす
        if (nowPlayerHPPersent < DamagePlayerHPPersent & 2 < nowTime) DamagePlayerHPPersent -= 1 * Time.deltaTime;

        // UIのデータのセット
        PlayerNowHP.value = nowPlayerHPPersent;
        PlayerLateHP.fillAmount = DamagePlayerHPPersent;
    }

    /// <summary>
    /// キー入力の表示
    /// </summary>
    public void MoveUI()
    {
        // 対応しているキーを入力していれば赤、していなければ黒にする
        if (_player.flont) KeyUI[0].color = Color.red; else KeyUI[0].color = Color.black;
        if (_player.back)  KeyUI[1].color = Color.red; else KeyUI[1].color = Color.black;
        if (_player.left)  KeyUI[2].color = Color.red; else KeyUI[2].color = Color.black;
        if (_player.right) KeyUI[3].color = Color.red; else KeyUI[3].color = Color.black;
        if (_player.run)   KeyUI[4].color = Color.red; else KeyUI[4].color = Color.black;

        // キーの名前を表示
        KeyText[0].text = Controller.Flont.ToString();
        KeyText[1].text = Controller.Back.ToString();
        KeyText[2].text = Controller.Left.ToString();
        KeyText[3].text = Controller.Right.ToString();
        KeyText[4].text = Controller.Run.ToString();
    }
}

using UnityEngine;
using UnityEngine.UI;
using AllGameManager;
using TMPro;

public class BattleUIManagement : MonoBehaviour
{
    // UIアセット
    [SerializeField, Tooltip("経過時間のテキスト")] TextMeshProUGUI timeText;
    [SerializeField, Tooltip("プレイヤーの現在のHPのスライダー")] Slider PlayerNowHP;
    [SerializeField, Tooltip("プレイヤーがダメージを受ける前のHPのUI")] Image PlayerLateHP;
    [SerializeField, Tooltip("敵がダメージを受けた時のUI")] GameObject damageUI;
    [NamedArray(new string[] { "Flont", "Back", "Left", "Right", "Sprint" }), SerializeField, Tooltip("キー入力のUI")]
    Image[] KeyUI;
    [NamedArray(new string[] { "Flont", "Back", "Left", "Right", "Sprint" }), SerializeField, Tooltip("キーの名前")]
    TextMeshProUGUI[] KeyText;

    // UIデータ
    /// <summary> プレイヤーのステータス </summary>
    private PlayerStatus _player;
    /// <summary> ダメージを受ける前のHPのパーセント </summary>
    private float LatePlayerHPPersent;
    /// <summary> ダメージを受けた分徐々に減らすHPのパーセント </summary>
    private float DamagePlayerHPPersent;
    /// <summary> ダメージを受けた時間 </summary>
    private float StartTime;
    /// <summary> ダメージを受けてからの経過時間 </summary>
    private float nowTime;

    /// <summary>
    /// プレイヤーステータスのセット
    /// </summary>
    /// <param name="player"> �v���C���[�X�e�[�^�X </param>
    public void SetStatus(PlayerStatus player) { _player = player; }

    /// <summary>
    /// ダメージUIの生成
    /// </summary>
    /// <param name="col"> 衝突したコライダーの地点 </param>
    /// <param name="damage"> ダメージ量 </param>
    public void DamageUI(Collider col, int damage)
    {
        // 衝突したコライダーの地点にUIを生成する
        GameObject _damageUI = Instantiate(damageUI, col.bounds.center - Camera.main.transform.forward * 0.2f, Quaternion.identity);

        // ダメージUIのTextをダメージ量に変える
        _damageUI.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
    }

    /// <summary>
    /// 経過時間の更新
    /// </summary>
    public void TimeTextUI(float time) { timeText.text = time.ToString(); }

    /// <summary>
    /// プレイヤーのHPのUI制御
    /// </summary>
    /// <param name="nowPlayerHPPersent"> プレイヤーの現在のHP </param>
    /// <param name="_LatePlayerHPPersent"> プレイヤーがダメージを受ける前のHP </param>
    public void PlayerHPUI(float nowPlayerHPPersent, float _LatePlayerHPPersent)
    {
        // ダメージを受ける前のHPが変化したか
        if (LatePlayerHPPersent != _LatePlayerHPPersent)
        {
            // ダメージを受ける前のHPを更新
            LatePlayerHPPersent = _LatePlayerHPPersent;

            // ダメージを受けた分徐々に減らすHPのセットアップ
            DamagePlayerHPPersent = _LatePlayerHPPersent;
            StartTime = Time.time;
        }

        // ダメージを受けてからの経過時間
        nowTime = Time.time - StartTime;

        // 現在のHPより徐々に減らすHPが多くてダメージの経過時間が2秒を越えていたら、HPが減る制御
        if (nowPlayerHPPersent < DamagePlayerHPPersent & 2 < nowTime) DamagePlayerHPPersent -= 1 * Time.deltaTime;

        // UIの制御
        PlayerNowHP.value = nowPlayerHPPersent;
        PlayerLateHP.fillAmount = DamagePlayerHPPersent;
    }

    /// <summary>
    /// キー入力によるUIの制御
    /// </summary>
    public void MoveUI()
    {
        // 対応しているキーが入力されたら赤に、入力が外れたら黒に色を変更する
        if (_player.flont) KeyUI[0].color = Color.red; else KeyUI[0].color = Color.black;
        if (_player.back)  KeyUI[1].color = Color.red; else KeyUI[1].color = Color.black;
        if (_player.left)  KeyUI[2].color = Color.red; else KeyUI[2].color = Color.black;
        if (_player.right) KeyUI[3].color = Color.red; else KeyUI[3].color = Color.black;
        if (_player.run)   KeyUI[4].color = Color.red; else KeyUI[4].color = Color.black;

        // 対応しているキーの名前をセット
        KeyText[0].text = Controller.Flont.ToString();
        KeyText[1].text = Controller.Back.ToString();
        KeyText[2].text = Controller.Left.ToString();
        KeyText[3].text = Controller.Right.ToString();
        KeyText[4].text = Controller.Run.ToString();
    }
}

using UnityEngine;
using UnityEngine.UI;
using AllGameManager;
using TMPro;

public class BattleUIManagement : MonoBehaviour
{
    // UI�A�Z�b�g
    /// <summary> �o�ߎ��� </summary>
    [SerializeField] TextMeshProUGUI timeText;
    /// <summary> �v���C���[�̌��݂�HP </summary>
    [SerializeField] Slider PlayerNowHP;
    /// <summary> �v���C���[�̏��X�Ɍ���HP�\�� </summary>
    [SerializeField] Image PlayerLateHP;
    /// <summary> �G�̃_���[�W�\�L </summary>
    [SerializeField] GameObject damageUI;
    /// <summary> �L�[���͂̕\�� </summary>
    [NamedArray(new string[] { "Flont", "Back", "Left", "Right", "Sprint" }), SerializeField]
    Image[] KeyUI;
    /// <summary> �L�[�̖��O </summary>
    [NamedArray(new string[] { "Flont", "Back", "Left", "Right", "Sprint" }), SerializeField]
    TextMeshProUGUI[] KeyText;

    // UI�ɕK�v�ȃf�[�^
    /// <summary> �v���C���[�̃L�[���� </summary>
    private PlayerStatus _player;
    /// <summary> �v���C���[�̃_���[�W���󂯂�O��HP�̃p�[�Z���g </summary>
    private float LatePlayerHPPersent;
    /// <summary> �_���[�W���󂯂Ă��珙�X�Ɍ���HP�̃p�[�Z���g </summary>
    private float DamagePlayerHPPersent;
    /// <summary> �v���C���[���_���[�W���󂯂��^�C�~���O��0�Ƃ������� </summary>
    private float StartTime;
    /// <summary> �v���C���[���_���[�W���󂯂Ă���o�߂������� </summary>
    private float nowTime;

    /// <summary>
    /// �v���C���[�̃X�e�[�^�X���L���b�V������
    /// </summary>
    /// <param name="player"> �v���C���[�X�e�[�^�X </param>
    public void SetStatus(PlayerStatus player) { _player = player; }

    /// <summary>
    /// �G�̃_���[�W�ʂ�\������
    /// </summary>
    /// <param name="col"> �_���[�W���󂯂��ꏊ </param>
    /// <param name="damage"> �_���[�W�� </param>
    public void DamageUI(Collider col, int damage)
    {
        // �G���_���[�W���󂯂��ꏊ�Ƀ_���[�W�ʂ̕\�LUI�𐶐�
        GameObject _damageUI = Instantiate(damageUI, col.bounds.center - Camera.main.transform.forward * 0.2f, Quaternion.identity);

        // ��������UI�̃e�L�X�g���_���[�W�ʂɕύX
        _damageUI.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
    }

    /// <summary>
    /// ���݂̌o�ߎ��Ԃ�\��
    /// </summary>
    /// <param name="time"> ���݂̌o�ߎ��� </param>
    public void TimeTextUI(float time) { timeText.text = time.ToString(); }

    /// <summary>
    /// �v���C���[��HP��UI
    /// </summary>
    /// <param name="nowPlayerHPPersent"> ���݂̃v���C���[��HP </param>
    /// <param name="_LatePlayerHPPersent"> �v���C���[�̃_���[�W���󂯂�O��HP </param>
    public void PlayerHPUI(float nowPlayerHPPersent, float _LatePlayerHPPersent)
    {
        // �_���[�W���󂯂�O��HP���ω�������
        if (LatePlayerHPPersent != _LatePlayerHPPersent)
        {
            // �_���[�W���󂯂�O��HP���X�V
            LatePlayerHPPersent = _LatePlayerHPPersent;

            // �_���[�W���󂯂�O��HP�������A���ݎ��Ԃ�0�Ƃ���
            DamagePlayerHPPersent = _LatePlayerHPPersent;
            StartTime = Time.time;
        }

        // �_���[�W���󂯂Ă���̌o�ߎ���
        nowTime = Time.time - StartTime;

        // ���݂�HP��菙�X�Ɍ��炷HP�������Čo�ߎ��Ԃ�2�b�o���Ă���΁A�_���[�W���󂯂�O��HP�����X�Ɍ��炷
        if (nowPlayerHPPersent < DamagePlayerHPPersent & 2 < nowTime) DamagePlayerHPPersent -= 1 * Time.deltaTime;

        // UI�̃f�[�^�̃Z�b�g
        PlayerNowHP.value = nowPlayerHPPersent;
        PlayerLateHP.fillAmount = DamagePlayerHPPersent;
    }

    /// <summary>
    /// �L�[���͂̕\��
    /// </summary>
    public void MoveUI()
    {
        // �Ή����Ă���L�[����͂��Ă���ΐԁA���Ă��Ȃ���΍��ɂ���
        if (_player.flont) KeyUI[0].color = Color.red; else KeyUI[0].color = Color.black;
        if (_player.back)  KeyUI[1].color = Color.red; else KeyUI[1].color = Color.black;
        if (_player.left)  KeyUI[2].color = Color.red; else KeyUI[2].color = Color.black;
        if (_player.right) KeyUI[3].color = Color.red; else KeyUI[3].color = Color.black;
        if (_player.run)   KeyUI[4].color = Color.red; else KeyUI[4].color = Color.black;

        // �L�[�̖��O��\��
        KeyText[0].text = Controller.Flont.ToString();
        KeyText[1].text = Controller.Back.ToString();
        KeyText[2].text = Controller.Left.ToString();
        KeyText[3].text = Controller.Right.ToString();
        KeyText[4].text = Controller.Run.ToString();
    }
}

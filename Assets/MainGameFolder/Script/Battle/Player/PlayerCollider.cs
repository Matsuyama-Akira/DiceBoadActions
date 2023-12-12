using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    // �K�{�X�N���v�g
    private PlayerStatus status;
    private PlayerController controller;

    /// <summary>
    /// �X�N���v�g�̃L���b�V��
    /// </summary>
    private void Awake()
    {
        status = GetComponentInParent<PlayerStatus>();
        controller = GetComponentInParent<PlayerController>();
    }

    /// <summary>
    /// ���̃X�N���v�g���K�p����Ă���R���C�_�[�ɏՓ˂����Ƃ��̏���
    /// </summary>
    /// <param name="other"> �Ώۂ̃R���C�_�[ </param>
    private void OnTriggerEnter(Collider other)
    {
        // �Փ˂����R���C�_�[�̃^�O��EnemyAttackCollider�Ȃ��
        if(other.gameObject.tag == "EnemyAttackCollider")
        {
            // EnemyAttack�̃q�b�g����0�Ȃ�΃_���[�W�������s��
            if (other.GetComponentInParent<EnemyAttack>().GetHit() < 1)
            {
                // �v���C���[�̃_���[�W���󂯂�O��HP����
                status.AddLateHP();

                // �󂯂��_���[�W��HP�����炷
                status.AddHP(other.GetComponentInParent<EnemyStates>().GetAttackDamage());

                // EnemyAttack�̃q�b�g����1���₷
                other.GetComponentInParent<EnemyAttack>().AddHit(1);

                // �v���C���[�̃_���[�W�A�j���[�V�������Đ�
                controller.AddHitAnim();
            }
        }
    }
}

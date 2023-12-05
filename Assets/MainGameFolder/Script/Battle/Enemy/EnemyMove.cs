using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Enemy all movement and animation
/// </summary>
public class EnemyMove : MonoBehaviour
{
    /// <summary> 現在のMoveステータス </summary>
    private enum Move
    {
        BodyTrackingIdle,
        FreeIdle,
        Tracking,
        Attack,
        Stan,
        Die,
    };

    [Header("アセット")]
    [SerializeField, Tooltip("ターゲットとなるオブジェクト")] GameObject target;
    [SerializeField, Tooltip("ターゲットのトランスフォーム")] Transform targetTransform;
    [SerializeField, Tooltip("このキャラクターの頭のトランスフォームを入れる")] Transform headTransform;
    [SerializeField, Tooltip("このキャラクターのNavMeshAgentを入れる")] NavMeshAgent agent;
    [SerializeField, Tooltip("このキャラクターのアニメーターを入れる")] Animator animator;
    [SerializeField, Tooltip("このキャラクターのEnemyStatesを入れる")] EnemyStates states;
    [SerializeField, Tooltip("このキャラクターのEnemyIsGroundedを入れる")] EnemyIsGrounded isGroundCheck;
    [SerializeField, Tooltip("このキャラクターのEnemyAttackを入れる")] EnemyAttack attack;

    [Space, Header("デバッグ用の表示ステータス")]
    [SerializeField, Tooltip("現在のトラッキング")] Move move;
    [SerializeField, Tooltip("ターゲットとの距離")] float distance;
    [SerializeField, Tooltip("前回再生したアニメーション")] string previewAnim;
    [SerializeField, Tooltip("現在再生中のアニメーション")] string nowAnim;

    //Private states.
    /// <summary> 視界内に入っているか </summary>
    public bool withinVisionRange { get; private set; }
    /// <summary> ターゲットが正面にいるか </summary>
    public bool isFlont { get; private set; }
    /// <summary> アニメーションが変わったか </summary>
    public bool animSwitch { get; private set; }
    /// <summary> 動いているか </summary>
    public bool isMoving { get; private set; }
    /// <summary> 着地しているか </summary>
    public bool isGround { get; private set; }
    /// <summary> 攻撃するか </summary>
    public bool isAttack { get; private set; }
    /// <summary> 攻撃しているか </summary>
    public bool isAttacking { get; private set; }
    /// <summary> 追跡しているか </summary>
    public bool isTracking { get; private set; }
    /// <summary> スタン中か </summary>
    public bool isStan { get; private set; }
    /// <summary> アニメーションがNullになっているか </summary>
    public bool isNullAnim { get; private set; }
    /// <summary> このキャラクターがターゲットを見ているか </summary>
    private bool looking;

    /// <summary> 再生中のアニメーションの名前 </summary>
    private AnimatorClipInfo[] clipInfos;

    void Awake()
    {
        target = GameObject.FindWithTag("PlayerTrackingPosition");
    }

    void FixedUpdate()
    {
        Math();
        CharactorControll();
        AnimatorControll();
        Debugger();
    }

    /// <summary>
    /// デバッグ用
    /// </summary>
    void Debugger()
    {
    }

    /// <summary>
    /// このキャラクターの動きを作る
    /// </summary>
    void CharactorControll()
    {
        Looking();
        ChangeMove();
        MoveControll();
    }

    /// <summary>
    /// このキャラクターのステータススイッチ
    /// </summary>
    void Math()
    {
        // ターゲットのトランスフォームを取得
        targetTransform = target.transform;
        // ターゲットとの距離を計算
        Vector3 direction = transform.position - targetTransform.position;
        distance = direction.magnitude;

        // 着地を確認
        isGround = isGroundCheck.isGrounded;
        // 動いているかを確認
        isMoving = agent.velocity.sqrMagnitude != 0;
        // 追跡しているかを確認
        isTracking = distance > agent.stoppingDistance;
        // 攻撃するかを確認
        isAttack = states.isDamage & !isTracking & isFlont;

        // 再生中のアニメーションを取得
        clipInfos = animator.GetCurrentAnimatorClipInfo(1);
        // アニメーションがNone(null)になっているかを確認
        isNullAnim = clipInfos.Length == 0;
        // None(null)ではない場合
        if (!isNullAnim)
        {
            // 攻撃中かを確認
            isAttacking = clipInfos[0].clip.name == "Attack_Craw_F";
            // スタン中かを確認
            isStan = clipInfos[0].clip.name == "Baruk_Hit_Back";
            // 現在のアニメーション名を取得
            nowAnim = clipInfos[0].clip.name.ToString();
            // 現在のアニメーションと前回のアニメーションが違うものかを確認
            animSwitch = nowAnim != previewAnim;
            // 前回のアニメーション名を更新
            previewAnim = clipInfos[0].clip.name.ToString();
        }

        // 攻撃中ではない場合、攻撃のヒット数をリセット
        if (!isAttacking & attack.GetHit() != 0) attack.ResetHit();
    }

    /// <summary>
    /// このキャラクターがどう動くかをスイッチする
    /// MoveControllに動き方とスイッチの仕方を記載しているため省略
    /// </summary>
    void ChangeMove()
    {
        if (states.isDie)
        {
            move = Move.Die;
            return;
        }
        if (isStan)
        {
            move = Move.Stan;
            return;
        }
        if (isAttack & isFlont)
        {
            move = Move.Attack;
            return;
        }
        if (!isAttacking & isTracking & (looking | states.isDamage))
        {
            move = Move.Tracking;
        }
        else
        {
            if (!isFlont & !isAttacking & states.isDamage) move = Move.BodyTrackingIdle;
            else move = Move.FreeIdle;
        }
    }

    /// <summary>
    /// ターゲットを見ているかの計算
    /// </summary>
    void Looking()
    {
        // このキャラクターの視界からターゲットがどの角度にいるかを計算する
        float dot = Vector3.Dot(transform.forward, (targetTransform.position - headTransform.position).normalized);

        // ステータスの視界内にターゲットがいるか
        withinVisionRange = dot > states.GetDot();
        // ターゲットが正面にいるか
        isFlont = dot > 0.7f;

        // 視界内にターゲットがいる場合
        if (withinVisionRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(headTransform.position, targetTransform.position - headTransform.position, out hit, Mathf.Infinity))
            {
                // プレイヤーとの間にオブジェクトがあるか
                switch (hit.collider.tag)
                {
                    case "Map":
                        Debug.DrawRay(headTransform.position, targetTransform.position - headTransform.position, Color.red, 1f);
                        looking = false;
                        return;
                    case "Player":
                        Debug.DrawRay(headTransform.position, targetTransform.position - headTransform.position, Color.blue, 1f);
                        looking = true;
                        return;
                    case "MainCamera":
                        Debug.DrawRay(headTransform.position, targetTransform.position - headTransform.position, Color.blue, 1f);
                        looking = true;
                        return;
                    default:
                        Debug.DrawRay(headTransform.position, targetTransform.position - headTransform.position, Color.blue, 1f);
                        looking = true;
                        return;

                }
            }
        }
    }

    /// <summary>
    /// このキャラクターを実際に動かす
    /// </summary>
    void MoveControll()
    {
        switch (move)
        {
            // 視界外にターゲットがいる場合とターゲットが範囲内かつ正面にいるかつダメージを受けていない場合、その場で待機する
            case Move.FreeIdle:
                agent.SetDestination(transform.position);
                break;

            // ターゲットが視界内にいるかダメージを受けていて、範囲内にいない場合ターゲットを追跡する
            case Move.Tracking:
                agent.SetDestination(targetTransform.position);
                break;

            // ダメージを受けていてターゲットが正面にいない場合、体の向きをターゲットに向ける
            case Move.BodyTrackingIdle:
                transform.LookAt(Vector3.Lerp(transform.forward + transform.position, targetTransform.position, 0.05f), Vector3.up);
                break;

            // ダメージを受けていてターゲットが範囲内かつ正面にいる場合、攻撃する
            case Move.Attack:
                if (!isAttacking) agent.SetDestination(targetTransform.position);
                else agent.SetDestination(transform.position);
                break;

            // 状態異常
            // スタン時はその場で止まりスタンアニメーションを再生
            case Move.Stan:
                agent.SetDestination(transform.position);
                break;
            // HPが0の時はその場で止まりアニメーションを再生
            case Move.Die:
                agent.SetDestination(transform.position);
                animator.SetBool("IsDie", states.isDie);
                break;
        }
    }

    /// <summary>
    /// アニメーションを動かす
    /// </summary>
    void AnimatorControll()
    {
        animator.SetFloat("MoveY", agent.velocity.sqrMagnitude);
        animator.SetBool("Moving", isMoving);
        animator.SetBool("IsGround", isGround);
        animator.SetBool("IsAttack", isAttack);
        if (states.isStan) animator.SetTrigger("IsHit");
    }
}
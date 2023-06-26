using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Enemy all movement and animation
/// </summary>
public class EnemyMove : MonoBehaviour
{
    [Tooltip("Move Sellect")]
    private enum Move
    {
        BodyTrackingIdle,
        FreeIdle,
        Tracking,
        Attack,
        Step,
        Stan,
        Die,
    };

    [Header("Asset")]
    [SerializeField, Tooltip("Target charactor object")]
    GameObject target;
    [SerializeField, Tooltip("Transforms to be tracked")]
    Transform targetTransform;
    [SerializeField, Tooltip("Transformation of this character's head")]
    Transform headTransform;
    [SerializeField, Tooltip("Insert the NavMeshAgent for this character.")]
    NavMeshAgent agent;
    [SerializeField, Tooltip("Insert the Animator for this character.")]
    Animator animator;
    [SerializeField, Tooltip("Insert the EnemyStates for this character.")]
    EnemyStates states;
    [SerializeField, Tooltip("Insert the EnemyIsGrounded for this character.")]
    EnemyIsGrounded isGroundCheck;
    [SerializeField, Tooltip("Insert the EnemyAttack for this character.")]
    EnemyAttack attack;

    [Space]
    [Header("Status for debugging")]
    [SerializeField] Move move;
    [SerializeField] float distance;
    public string previewAnim;
    public string nowAnim;

    //Private states.
    /// <summary> It's in view. </summary>
    public bool frag { get; private set; }
    /// <summary> Target's right out front. </summary>
    public bool isFlont { get; private set; }
    /// <summary> Switching animation. </summary>
    public bool animSwitch { get; private set; }
    /// <summary> It's moving. </summary>
    public bool isMoving { get; private set; }
    /// <summary> On the ground.</summary>
    public bool isGround { get; private set; }
    /// <summary> Attack. </summary>
    public bool isAttack { get; private set; }
    /// <summary> Attacking now. </summary>
    public bool isAttacking { get; private set; }
    /// <summary> Tracking now </summary>
    public bool isTracking { get; private set; }
    /// <summary> Inability to act. </summary>
    public bool isStan { get; private set; }
    /// <summary> Null animation. </summary>
    public bool isNullAnim { get; private set; }
    /// <summary> This character is looking at the target. </summary>
    private bool looking;

    private AnimatorClipInfo[] clipInfos;

    private Vector3 velocity = Vector3.zero;


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

    void Debugger()
    {
    }

    /// <summary>
    /// Charactor all controlling
    /// </summary>
    void CharactorControll()
    {
        Looking();
        ChangeMove();
        MoveControll();
    }

    /// <summary>
    /// States 
    /// </summary>
    void Math()
    {
        targetTransform = target.transform;
        clipInfos = animator.GetCurrentAnimatorClipInfo(1);                 //
        Vector3 direction = transform.position - targetTransform.position;  //
        distance = direction.magnitude;                                     //
        isGround = isGroundCheck.isGrounded;                                //
        isMoving = agent.velocity.sqrMagnitude != 0;                        //
        isTracking = distance > agent.stoppingDistance;                     //
        isAttack = states.isDamage & !isTracking & isFlont;                 //
        isNullAnim = clipInfos.Length == 0;                                 //
        if (!isNullAnim)                                                    //
        {
            isAttacking = clipInfos[0].clip.name == "Attack_Craw_F";        //
            isStan = clipInfos[0].clip.name == "Baruk_Hit_Back";
            nowAnim = clipInfos[0].clip.name.ToString();                    //
            animSwitch = nowAnim != previewAnim;                            //
            previewAnim = clipInfos[0].clip.name.ToString();                //
        }
        if (!isAttacking) attack.ResetHit();
    }

    /// <summary>
    /// Chenging move
    /// </summary>
    void ChangeMove()
    {
        if (isStan)
        {
            move = Move.Stan;
            return;
        }
        if (states.isDie)
        {
            move = Move.Die;
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
    /// Looking for player
    /// </summary>
    void Looking()
    {
        float dot = Vector3.Dot(transform.forward, (targetTransform.position - headTransform.position).normalized);
        frag = dot > states.GetDot();
        isFlont = dot > 0.7f;
        if (frag)
        {
            RaycastHit hit;
            if (Physics.Raycast(headTransform.position, targetTransform.position - headTransform.position, out hit, Mathf.Infinity))
            {
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
    /// Move switcher
    /// </summary>
    void MoveControll()
    {
        switch (move)
        {
            case Move.BodyTrackingIdle:
                transform.LookAt(Vector3.Lerp(transform.forward + transform.position, targetTransform.position, 0.05f), Vector3.up);
                break;
            case Move.FreeIdle:
                agent.SetDestination(transform.position);
                break;
            case Move.Tracking:
                agent.SetDestination(targetTransform.position);
                break;
            case Move.Attack:
                if (!isAttacking) agent.SetDestination(targetTransform.position);
                else agent.SetDestination(transform.position);
                break;
            case Move.Step:
                animator.SetTrigger("Dodge");
                break;
            case Move.Stan:
                agent.SetDestination(transform.position);
                break;
            case Move.Die:
                agent.SetDestination(transform.position);
                animator.SetBool("IsDie", states.isDie);
                break;
        }
    }

    /// <summary>
    /// Animation setter
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
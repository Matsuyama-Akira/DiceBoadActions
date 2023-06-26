using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Asset")]
    [SerializeField] Animator animator;
    [SerializeField] PlayerStatus status;

    [Space, Header("Movement Status")]
    [SerializeField, Range(1, 10)] private float speed = 5;
    [SerializeField, Range(1f, 100f)] float jumpStrength;
    [SerializeField, Range(0.1f, 1f)] float jumpMove;
    public event System.Action Jumped;
    public event System.Action Attacked;
    public event System.Action UniqueAttacked;
    [SerializeField, Range(2, 20)] float runSpeed = 9;

    //private states
    /// <summary> Amount of diagonal movement </summary>
    private float sqrtMove = (float)(1 / Math.Sqrt(2));
    /// <summary> Amount of movement in front or directly behind </summary>
    public float moveX { get; private set; }
    /// <summary> Amount of movement in right or left </summary>
    public float moveY { get; private set; }
    /// <summary> Now play animation </summary>
    private string nowAnim;
    /// <summary>  </summary>

    private AnimatorClipInfo[] clipInfos;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();



    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Movement();
        Animation();
        Debugger();
    }

    void Movement()
    {
        Status();
        Move();
    }
    void Move()
    {
        // Get targetMovingSpeed.
        float targetMovingSpeed = status.run ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity = new Vector2(moveX * targetMovingSpeed, moveY * targetMovingSpeed);
        Jump();

        // Apply movement.
        rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
    }
    void Status()
    {
        GetNowPlayAnim();
        MoveStatus();
    }
    private void GetNowPlayAnim()
    {
        clipInfos = animator.GetCurrentAnimatorClipInfo(1);
        if(clipInfos.Length != 0) nowAnim = clipInfos[0].clip.name.ToString();
    }
    private void MoveStatus()
    {
        MoveX();
        MoveY();
    }

    private void MoveX()
    {
        moveX = status.right ?  1 : moveX;
        moveX = status.left  ? -1 : moveX;
        moveX = status.right & (status.flont | status.back) ? sqrtMove : moveX;
        moveX = status.right & status.flont & status.back ? 1 : moveX;
        moveX = status.left  & (status.flont | status.back) ? -sqrtMove : moveX;
        moveX = status.left  & status.flont & status.back ? -1 : moveX;
        moveX = (status.right & status.left) | (!status.right & !status.left) ? 0 : moveX;
        moveX = status.unique ? 0 : moveX;
    }
    private void MoveY()
    {
        moveY = status.flont ?  1 : moveY;
        moveY = status.back  ? -1 : moveY;
        moveY = status.flont & (status.right | status.left) ? sqrtMove : moveY;
        moveY = status.flont & status.right & status.left ? 1 : moveY;
        moveY = status.back  & (status.right | status.left) ? -sqrtMove : moveY;
        moveY = status.back  & status.right & status.left ? -1 : moveY;
        moveY = (status.flont & status.back) | (!status.flont & !status.back) ? 0 : moveY;
        moveY = status.unique ? 0 : moveY;
    }

    private void Jump()
    {
        if (status.jump)
        {
            rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
            Jumped?.Invoke();
        }
    }

    void Animation()
    {
        animator.SetFloat("Move", moveY);
        animator.SetBool("Run", status.run);
        animator.SetBool("Die", status.isDie);
        animator.SetBool("Unique", status.unique);
        animator.SetBool("Attack", status.attack);
    }

    public void AddHitAnim()
    {
        animator.SetTrigger("Hit");
    }


    public string GetNowAnim() { return nowAnim; }

    void Debugger()
    {
    }
}

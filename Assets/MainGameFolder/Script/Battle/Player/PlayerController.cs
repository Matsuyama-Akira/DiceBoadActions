using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerStatus))]
public class PlayerController : MonoBehaviour
{
    [Header("Asset")]
    [SerializeField, Tooltip("アニメーターを挿入")] Animator animator;
    [SerializeField, Tooltip("PlayerStatusを挿入")] PlayerStatus status;

    [Space, Header("Movement Status")]
    [SerializeField, Range(1, 10), Tooltip("歩行の移動速度")] private float speed = 5;
    [SerializeField, Range(1f, 100f), Tooltip("ジャンプの高さ")] float jumpStrength;
    [SerializeField, Range(0.1f, 1f), Tooltip("空中の移動速度")] float jumpMove;
    [SerializeField, Range(2, 20), Tooltip("ダッシュの移動速度")] float runSpeed = 9;

    // プライベートのステータス
    /// <summary> 斜め移動の倍率 </summary>
    private float sqrtMove = (float)(1 / Math.Sqrt(2));
    /// <summary> 横移動の倍率 </summary>
    public float moveX { get; private set; }
    /// <summary> 縦移動の倍率 </summary>
    public float moveY { get; private set; }
    /// <summary> 再生中のアニメーション名 </summary>
    private string nowAnim;

    /// <summary> 再生中のアニメーション </summary>
    private AnimatorClipInfo[] clipInfos;

    /// <summary> このオブジェクトの物理演算 </summary>
    private Rigidbody rigidbody;

    /// <summary> 移動速度を最後に追加された速度に上書きする </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    /// <summary>
    /// 各種コンポーネントのキャッシュ
    /// </summary>
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GameObject.Find("Male A Variant").GetComponent<Animator>();
        status = GetComponent<PlayerStatus>();
    }

    void FixedUpdate()
    {
        Movement();
        Animation();
        Debugger();
    }

    /// <summary>
    /// このキャラクターの挙動やアニメーション
    /// </summary>
    void Movement()
    {
        GetNowPlayAnim();
        MoveStatus();
        Move();
    }

    /// <summary>
    /// オブジェクトの移動
    /// </summary>
    void Move()
    {
        // 移動速度の更新
        float targetMovingSpeed = status.run ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // 移動距離を計算
        Vector2 targetVelocity = new Vector2(moveX * targetMovingSpeed, moveY * targetMovingSpeed);

        // 実際にオブジェクトを動かす
        Jump();
        rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
    }

    /// <summary>
    /// 現在のアニメーション名の取得
    /// </summary>
    private void GetNowPlayAnim()
    {
        // アニメーションを取得
        clipInfos = animator.GetCurrentAnimatorClipInfo(1);
        // アニメーションがNone(null)でない場合、アニメーション名を取得
        if(clipInfos.Length != 0) nowAnim = clipInfos[0].clip.name.ToString();
    }

    /// <summary>
    /// キー入力による移動速度の処理
    /// </summary>
    private void MoveStatus()
    {
        MoveX();
        MoveY();
    }

    /// <summary>
    /// 横移動の処理
    /// </summary>
    private void MoveX()
    {
        // 左右どちらかのみ
        moveX = status.right ?  1 : moveX;
        moveX = status.left  ? -1 : moveX;

        // 左右どちらかと上下どちらか
        moveX = status.right & (status.flont ^ status.back) ? sqrtMove : moveX;
        // moveX = status.right & status.flont & status.back ? 1 : moveX;
        moveX = status.left  & (status.flont ^ status.back) ? -sqrtMove : moveX;
        // moveX = status.left  & status.flont & status.back ? -1 : moveX;

        // 左右同じ入力、またはユニーク入力
        moveX = !(status.right ^ status.left) | status.unique ? 0 : moveX;
        // moveX = (status.right & status.left) | (!status.right & !status.left) ? 0 : moveX;
        // moveX = status.unique ? 0 : moveX;
    }

    /// <summary>
    /// 縦移動の処理
    /// </summary>
    private void MoveY()
    {
        // 上下どちらかのみ
        moveY = status.flont ?  1 : moveY;
        moveY = status.back  ? -1 : moveY;

        // 上下どちらかと左右どちらか
        moveY = status.flont & (status.right ^ status.left) ? sqrtMove : moveY;
        // moveY = status.flont & status.right & status.left ? 1 : moveY;
        moveY = status.back  & (status.right ^ status.left) ? -sqrtMove : moveY;
        // moveY = status.back  & status.right & status.left ? -1 : moveY;

        // 上下同じ入力、またはユニーク入力
        moveY = !(status.flont ^ status.back) | status.unique ? 0 : moveY;
        // moveY = (status.flont & status.back) | (!status.flont & !status.back) ? 0 : moveY;
        // moveY = status.unique ? 0 : moveY;
    }

    /// <summary>
    /// ジャンプの動作
    /// </summary>
    private void Jump()
    {
        if (status.jump)
        {
            // 上方向に力を加える
            rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
        }
    }

    /// <summary>
    /// 対応するアニメーションの変数をセット
    /// </summary>
    void Animation()
    {
        animator.SetFloat("Move", moveY);
        animator.SetBool("Run", status.run);
        animator.SetBool("Die", status.isDie);
        animator.SetBool("Unique", status.unique);
        animator.SetBool("Attack", status.attack);
    }

    /// <summary>
    /// コライダーがヒットしたらアニメーションを再生
    /// </summary>
    public void AddHitAnim()
    {
        animator.SetTrigger("Hit");
    }

    /// <summary>
    /// 現在のアニメーション名を返す
    /// </summary>
    /// <returns> 現在のアニメーション名 </returns>
    public string GetNowAnim() { return nowAnim; }

    void Debugger()
    {
    }
}

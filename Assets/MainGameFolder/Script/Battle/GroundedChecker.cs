using UnityEngine;

[ExecuteInEditMode]
public class GroundedChecker : MonoBehaviour
{
    [Tooltip("地面からの最大距離")]
    public float distanceThreshold = .15f;

    [Tooltip("着地しているか")]
    public bool isGrounded = true;
    /// <summary> 再着地時にコール </summary>
    public event System.Action Grounded;

    const float OriginOffset = 0.001f;
    Vector3 RaycastOrigin => transform.position + Vector3.up * OriginOffset;
    float RaycastDistance => distanceThreshold + OriginOffset;

    void LateUpdate()
    {
        // 着地しているかを確認
        bool isGroundedNow = Physics.Raycast(RaycastOrigin, Vector3.down, distanceThreshold * 2);

        // 再着地したならイベントをコール
        if (isGroundedNow && !isGrounded)
        {
            Grounded?.Invoke();
        }

        // 着地情報を更新
        isGrounded = isGroundedNow;
    }

    void OnDrawGizmosSelected()
    {
        // エディター上にRaycastを表示し、着地しているかを確認
        Debug.DrawLine(RaycastOrigin, RaycastOrigin + Vector3.down * RaycastDistance, isGrounded ? Color.white : Color.red);
    }
}

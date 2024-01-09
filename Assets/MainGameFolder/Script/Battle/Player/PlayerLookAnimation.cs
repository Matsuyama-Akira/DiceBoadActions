using UnityEngine;

public class PlayerLookAnimation : MonoBehaviour
{
    [SerializeField] Transform spine;
    [SerializeField] PlayerCamera cameraStates;
    [SerializeField] Transform mainCamera;
    [SerializeField] Transform cameraPosition;

    [SerializeField] float cameraRotateLimit = 30f;

    /// <summary> キャラクターのY軸の角度 </summary>
    private Quaternion initCameraRot;
    /// <summary> カメラのX軸の角度 </summary>
    private Quaternion cameraRotate;

    private void Start()
    {
        initCameraRot = mainCamera.localRotation;
        cameraRotate = mainCamera.localRotation;
    }

    private void LateUpdate()
    {
        RotateBone();
        RotateCamera();

        // カメラの位置と角度を変更
        mainCamera.localRotation = cameraPosition.localRotation;
        mainCamera.position = cameraPosition.position;
    }

    void RotateBone()
    {
        // 腰のボーンの角度をカメラの向きに合わせる
        spine.rotation = Quaternion.Euler(spine.eulerAngles.x, spine.eulerAngles.y, spine.eulerAngles.z + -mainCamera.localEulerAngles.x);
    }

    void RotateCamera()
    {
        // カメラの角度を計算
        cameraRotate *= Quaternion.Euler(mainCamera.rotation.x, 0f, 0f);

        // カメラのX軸の角度が限界角度を超えたら限界角度に
        var resultYRot = Mathf.Clamp(Mathf.DeltaAngle(initCameraRot.eulerAngles.x, cameraRotate.eulerAngles.x), -cameraRotateLimit, cameraRotateLimit);

        // 角度を再構築
        cameraRotate = Quaternion.Euler(resultYRot, cameraRotate.eulerAngles.y, cameraRotate.eulerAngles.z);

        // カメラの視点移動を実行
        cameraPosition.localRotation = Quaternion.Slerp(mainCamera.localRotation, cameraRotate, cameraStates.sensitivity * Time.deltaTime);
    }
}

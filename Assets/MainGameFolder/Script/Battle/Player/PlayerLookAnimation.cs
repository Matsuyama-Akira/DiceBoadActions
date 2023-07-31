using UnityEngine;

public class PlayerLookAnimation : MonoBehaviour
{
    [SerializeField] Transform spine;
    [SerializeField] PlayerCamera cameraStates;
    [SerializeField] Transform mainCamera;
    [SerializeField] Transform cameraPosition;

    [SerializeField] float cameraRotateLimit = 30f;

    private Quaternion initCameraRot;
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

        mainCamera.localRotation = cameraPosition.localRotation;
        mainCamera.position = cameraPosition.position;
    }

    void RotateBone()
    {
        spine.rotation = Quaternion.Euler(spine.eulerAngles.x, spine.eulerAngles.y, spine.eulerAngles.z + -mainCamera.localEulerAngles.x);
    }
    //おそらくここのどこかで不具合を起こしているので要検証
    void RotateCamera()
    {
        //float xRotate = cameraStates.mouseDeltaY;

        //xRotate *= -1;
        //　一旦角度を計算する	
        cameraRotate *= Quaternion.Euler(mainCamera.rotation.x, 0f, 0f);
        //　カメラのX軸の角度が限界角度を超えたら限界角度に設定
        var resultYRot = Mathf.Clamp(Mathf.DeltaAngle(initCameraRot.eulerAngles.x, cameraRotate.eulerAngles.x), -cameraRotateLimit, cameraRotateLimit);
        Debug.Log(resultYRot);
        //　角度を再構築
        cameraRotate = Quaternion.Euler(resultYRot, cameraRotate.eulerAngles.y, cameraRotate.eulerAngles.z);
        //　カメラの視点変更を実行
        cameraPosition.localRotation = Quaternion.Slerp(mainCamera.localRotation, cameraRotate, cameraStates.sensitivity * Time.deltaTime);
    }
}

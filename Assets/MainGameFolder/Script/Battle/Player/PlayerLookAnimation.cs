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

    void RotateCamera()
    {
        cameraRotate *= Quaternion.Euler(mainCamera.rotation.x, 0f, 0f);
        var resultYRot = Mathf.Clamp(Mathf.DeltaAngle(initCameraRot.eulerAngles.x, cameraRotate.eulerAngles.x), -cameraRotateLimit, cameraRotateLimit);
        cameraRotate = Quaternion.Euler(resultYRot, cameraRotate.eulerAngles.y, cameraRotate.eulerAngles.z);
        cameraPosition.localRotation = Quaternion.Slerp(mainCamera.localRotation, cameraRotate, cameraStates.sensitivity * Time.deltaTime);
    }
}

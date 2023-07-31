using UnityEngine;
using AllGameManager;

public class PlayerCamera : MonoBehaviour
{
    public enum CameraType
    {
        Position, MoveRange,
    }
    [SerializeField] Transform character;
    [SerializeField] CameraType type = CameraType.MoveRange;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;
    Vector2 rotationAmount;

    bool cursorLock;

    public float mouseDeltaY;

    void Update()
    {
        switch (type)
        {
            case CameraType.Position:
                MoucePositionCamera();
                break;
            case CameraType.MoveRange:
                MouceMoveRangeCamera();
                break;
        }
        UpdateCursorLock();
    }
    void MoucePositionCamera()
    {
        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDeltaY = mouseDelta.y;
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * Controller.AllSensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        // Rotate camera up-down and controller left-right from velocity.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y * Controller.SensiY, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x * Controller.SensiX, Vector3.up);
    }
    void MouceMoveRangeCamera()
    {
        // マウスの移動量を取得
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // マウスの移動量に応じてカメラを回転させる
        rotationAmount = new Vector2(mouseX, -mouseY) * sensitivity;
        if (transform.rotation.x <= -90 | transform.rotation.x >= 90) rotationAmount.y = 0;
        transform.localRotation *= Quaternion.AngleAxis(rotationAmount.y, Vector3.right);
        character.localRotation *= Quaternion.AngleAxis(rotationAmount.x, Vector3.up);
    }

    void UpdateCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLock = false;
        }
        else if (Input.GetMouseButton(0))
        {
            cursorLock = true;
        }


        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!cursorLock)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

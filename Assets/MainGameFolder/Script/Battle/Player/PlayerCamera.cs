using UnityEngine;
using AllGameManager;

public class PlayerCamera : MonoBehaviour
{
    // マウスの移動量の取得方法
    public enum CameraType
    {
        Position, MoveRange,
    }

    [SerializeField, Tooltip("プレイヤーキャラクターのトランスフォーム")] Transform character;
    [SerializeField, Tooltip("取得方法の選択")] CameraType type = CameraType.Position;
    /// <summary> マウスの感度 </summary>
    public float sensitivity = 2;
    /// <summary>  </summary>
    public float smoothing = 1.5f;

    // マウスの位置で移動量を取得する場合に使う数値
    /// <summary> 実際にカメラを動かす量 </summary>
    Vector2 velocity;
    /// <summary> マウスの移動量 </summary>
    Vector2 frameVelocity;

    // マウスの速度で移動量を取得する場合に使う数値
    /// <summary> マウスの移動量 </summary>
    Vector2 rotationAmount;

    /// <summary> カーソルをロックしているか </summary>
    bool cursorLock;

    /// <summary> マウスのY軸の移動量 </summary>
    public float mouseDeltaY;

    void Update()
    {
        // マウスの移動量の取得方法で計算方法を変える
        switch (type)
        {
            // マウスの位置から
            case CameraType.Position:
                MoucePositionCamera();
                break;
            // マウスの速度から
            case CameraType.MoveRange:
                MouceMoveRangeCamera();
                break;
        }

        // カーソルのオンオフ
        UpdateCursorLock();
    }
    void MoucePositionCamera()
    {

        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDeltaY = mouseDelta.y;
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * Controller.AllSensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(-velocity.y * Controller.SensiY, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x * Controller.SensiX, Vector3.up);
    }
    void MouceMoveRangeCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

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

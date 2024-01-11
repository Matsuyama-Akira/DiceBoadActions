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
                MouseMoveRangeCamera();
                break;
        }

        // カーソルのオンオフ
        UpdateCursorLock();
    }
    void MoucePositionCamera()
    {
        // マウスの移動量を取得
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDeltaY = mouseDelta.y;

        // 数値の補正
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * Controller.AllSensitivity);

        // 前フレームの位置と現フレームの位置から直線で補間する
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);

        // 位置の更新　Y軸は-90~90の間にする
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        // Y軸はカメラを動かし、X軸はキャラクターを動かす
        transform.localRotation = Quaternion.AngleAxis(-velocity.y * Controller.SensiY, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x * Controller.SensiX, Vector3.up);
    }
    void MouseMoveRangeCamera()
    {
        // マウスの移動量を取得
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        rotationAmount = new Vector2(mouseX, -mouseY) * sensitivity;

        // カメラのX軸回転が-90以下もしくは90以上ならば、Y軸の移動量を0にする
        if (transform.rotation.x <= -90 | transform.rotation.x >= 90) rotationAmount.y = 0;

        // Y軸はカメラを動かし、X軸はキャラクターを動かす
        transform.localRotation *= Quaternion.AngleAxis(rotationAmount.y, Vector3.right);
        character.localRotation *= Quaternion.AngleAxis(rotationAmount.x, Vector3.up);
    }

    void UpdateCursorLock()
    {
        // マウスカーソルの表示処理
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLock = false;
        }
        if (Input.GetMouseButton(0))
        {
            cursorLock = true;
        }


        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

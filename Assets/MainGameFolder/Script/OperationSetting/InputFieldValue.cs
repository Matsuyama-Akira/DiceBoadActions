using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputFieldValue : MonoBehaviour
{
    [SerializeField, Tooltip("ここからテキストを取得する")] TMP_InputField field;
    [SerializeField, Tooltip("取得した数値を適用させる")] Slider SensitivityValue;
    /// <summary> テキストが変更中かの確認 </summary>
    public bool playInput;

    private void Update()
    {
        // テキストを取得
        if (!playInput)
        {
            field.text = SensitivityValue.value.ToString("0.000");
        }
    }

    public void GetInputName()
    {
        // 取得したテキストをfloatにしてその数値をカメラ感度に適用
        float value = float.Parse(field.text);
        SensitivityValue.value = value;
        playInput = false;
    }

    public void TryInputName()
    {
        // インプットフィールドを選択中
        playInput = true;
    }
}

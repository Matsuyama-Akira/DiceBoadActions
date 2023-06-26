using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputFieldValue : MonoBehaviour
{
    [SerializeField] TMP_InputField field;
    [SerializeField] Slider allSensiValue;
    public bool playInput;

    private void Update()
    {
        if (!playInput)
        {
            field.text = allSensiValue.value.ToString("0.000");
        }
    }

    public void GetInputName()
    {
        float value = float.Parse(field.text);
        allSensiValue.value = value;
        playInput = false;
    }

    public void TryInputName()
    {
        playInput = true;
    }
}

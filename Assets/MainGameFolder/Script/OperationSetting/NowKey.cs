using UnityEngine;
using AllGameManager;
using TMPro;

[ExecuteInEditMode]
public class NowKey : MonoBehaviour
{
    public enum ThisKey
    {
        Flont, Back, Right, Left, Attack, Unique, Skill, Jump, Crouch, Run,
    }

    private TextMeshProUGUI[] KeyText;
    [SerializeField] ThisKey key;

    private void Awake()
    {
        KeyText = GetComponentsInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        switch (key)
        {
            case ThisKey.Flont:
                KeyText[1].text = Controller.Flont.ToString();
                break;
            case ThisKey.Back:
                KeyText[1].text = Controller.Back.ToString();
                break;
            case ThisKey.Right:
                KeyText[1].text = Controller.Right.ToString();
                break;
            case ThisKey.Left:
                KeyText[1].text = Controller.Left.ToString();
                break;
            case ThisKey.Attack:
                KeyText[1].text = Controller.Attack.ToString();
                break;
            case ThisKey.Unique:
                KeyText[1].text = Controller.Unique.ToString();
                break;
            case ThisKey.Skill:
                KeyText[1].text = Controller.Skill.ToString();
                break;
            case ThisKey.Jump:
                KeyText[1].text = Controller.Jump.ToString();
                break;
            case ThisKey.Crouch:
                KeyText[1].text = Controller.Crouch.ToString();
                break;
            case ThisKey.Run:
                KeyText[1].text = Controller.Run.ToString();
                break;
        }
        KeyText[0].text = key.ToString();
        gameObject.name = key.ToString();
    }
}

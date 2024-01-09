using UnityEngine;
using AllGameManager;
using TMPro;

[ExecuteInEditMode]
public class NowKey : MonoBehaviour
{
    /// <summary> コントロール </summary>
    public enum ThisKey
    {
        Flont, Back, Right, Left, Attack, Unique, Skill, Jump, Crouch, Run,
    }

    /// <summary> 表示テキスト </summary>
    private TextMeshProUGUI[] KeyText;
    /// <summary> このオブジェクトのコントロール </summary>
    [SerializeField] ThisKey key;

    /// <summary> テキストの改行までの文字数 </summary>
    private int stringSprint;

    private void Awake()
    {
        // テキストの入れ子を取得
        KeyText = GetComponentsInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        // このオブジェクトのコントロールに対応しているキーを取得しテキストに挿入
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

        // キーの名前をテキストに挿入
        KeyText[0].text = key.ToString();

        // 改行
        StringNewLine();

        // オブジェクトの名前をコントロールの名前に変更
        gameObject.name = key.ToString();
    }

    private void StringNewLine()
    {
        // キーの名前に"Right"が入っていたら改行までの文字数を5に
        if (KeyText[1].text.IndexOf("Right") != -1) stringSprint = 5;
        // "Left"が入っていたら4に
        else if (KeyText[1].text.IndexOf("Left") != -1) stringSprint = 4;
        // それ以外は変更なしで
        else return;

        // 改行までの文字数以降の文字列を取得し、後ろの文字列の前に改行を挿入する
        string backChar = KeyText[1].text.Substring(stringSprint);
        KeyText[1].text = KeyText[1].text.Remove(stringSprint, backChar.Length) + "\n" + backChar;
    }
}

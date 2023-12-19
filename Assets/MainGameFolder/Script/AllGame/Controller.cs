using UnityEngine;

namespace AllGameManager
{
    public class Controller : MonoBehaviour
    {
        // 通常の移動キーの設定
        /// <summary> 前方に動かすキー。デフォルトは"W" </summary>
        static public KeyCode Flont  { get; set; } = KeyCode.W;
        /// <summary> 後方に動かすキー。デフォルトは"S" </summary>
        static public KeyCode Back   { get; set; } = KeyCode.S;
        /// <summary> 右方に動かすキー。デフォルトは"D" </summary>
        static public KeyCode Right  { get; set; } = KeyCode.D;
        /// <summary> 左方に動かすキー。デフォルトは"A" </summary>
        static public KeyCode Left   { get; set; } = KeyCode.A;

        // 特殊な移動キーの設定
        /// <summary> ジャンプするキー。デフォルトは"Space" </summary>
        static public KeyCode Jump   { get; set; } = KeyCode.Space;
        /// <summary> しゃがむキー。デフォルトは"LeftControl" </summary>
        static public KeyCode Crouch { get; set; } = KeyCode.LeftControl;
        /// <summary> 走るキー。デフォルトは"LeftShft" </summary>
        static public KeyCode Run    { get; set; } = KeyCode.LeftShift;

        // バトルでの攻撃キーの設定
        /// <summary> 通常攻撃のキー。デフォルトは"LeftMouce" </summary>
        static public KeyCode Attack { get; set; } = KeyCode.Mouse0;
        /// <summary> 特殊攻撃のキー。デフォルトは"RightMouce" </summary>
        static public KeyCode Unique { get; set; } = KeyCode.Mouse1;
        /// <summary> アビリティなどのキー。デフォルトは"Q" </summary>
        static public KeyCode Skill  { get; set; } = KeyCode.Q;

        // 感度設定
        /// <summary> 横軸のマウス感度。デフォルトの数値は"1.000" </summary>
        static public float SensiX   { get; set; } = 1.0f;
        /// <summary> 縦軸のマウス感度。デフォルトの数値は"1.000" </summary>
        static public float SensiY   { get; set; } = 1.0f;
        /// <summary> 全体のマウス感度。デフォルトの数値は"2.000" </summary>
        static public float AllSensitivity { get; set; } = 2.0f;
    }
}
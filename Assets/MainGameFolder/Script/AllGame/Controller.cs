using UnityEngine;

namespace AllGameManager
{
    public class Controller : MonoBehaviour
    {
        //Move controll
        //Forward/backward/left/right movement
        /// <summary> Keys for Flont move. Default key "W". </summary>
        static public KeyCode Flont  { get; set; } = KeyCode.W;
        /// <summary> Keys for Back move. Default key "S". </summary>
        static public KeyCode Back   { get; set; } = KeyCode.S;
        /// <summary> Keys for Right move. Default key "D". </summary>
        static public KeyCode Right  { get; set; } = KeyCode.D;
        /// <summary> Keys for Left move. Default key "A". </summary>
        static public KeyCode Left   { get; set; } = KeyCode.A;

        //Special Movements
        /// <summary> Keys for Jump. Default key "Space". </summary>
        static public KeyCode Jump   { get; set; } = KeyCode.Space;
        /// <summary> Keys for Crouch. Default key "LeftControl". </summary>
        static public KeyCode Crouch { get; set; } = KeyCode.LeftControl;
        /// <summary> Keys for Run. Default key "LeftShift". </summary>
        static public KeyCode Run    { get; set; } = KeyCode.LeftShift;

        //Attacks in Battle
        /// <summary> Keys for Main attack. Default key "LeftMouse". </summary>
        static public KeyCode Attack { get; set; } = KeyCode.Mouse0;
        /// <summary> Keys for Unique attack. Default key "RightMouse". </summary>
        static public KeyCode Unique { get; set; } = KeyCode.Mouse1;
        /// <summary> Keys for Skill attack. Default key "Q". </summary>
        static public KeyCode Skill  { get; set; } = KeyCode.Q;

        //Sensitivity
        /// <summary> Mouse sensitivity for Vector X. Default value "1.000". </summary>
        static public float SensiX   { get; set; } = 1.0f;
        /// <summary> Mouse sensitivity for Vector Y. Default value "1.000". </summary>
        static public float SensiY   { get; set; } = 1.0f;
        /// <summary> Mouse sensitivity for All. Default value "2.000". </summary>
        static public float AllSensitivity { get; set; } = 2.0f;
    }
}
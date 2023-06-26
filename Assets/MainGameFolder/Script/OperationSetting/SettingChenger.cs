using UnityEngine;
using UnityEngine.UI;
using AllGameManager;
using NextSceneScript;
using System;

public class SettingChenger : MonoBehaviour
{
    private enum KeySellect
    {
        Flont, Back, Right, Left, Jump, Crouch, Run, Attack, Skill, Unique, None,
    }

    private AllGameManagement gameManager;
    private AllGameSEManager seManager;
    private NextScene sceneChenger;
    private GameObject inputKeyCheckUI;
    private Slider SensitivityXSlider;
    private Slider SensitivityYSlider;
    private Slider allSensitivitySlider;

    //Sensi set
    [SerializeField] private float setSensiX;
    [SerializeField] private float setSensiY;
    [SerializeField] private float setAllSensi;

    //Key set
    private KeyCode setFlont;
    private KeyCode setBack;
    private KeyCode setRight;
    private KeyCode setLeft;

    private KeyCode setJump;
    private KeyCode setCrouch;
    private KeyCode setRun;

    private KeyCode setAttack;
    private KeyCode setSkill;
    private KeyCode setUnique;

    //Selecting key
    private KeySellect sellect = KeySellect.None;
    private bool resetter;

    private void Awake()
    {
        GameObject manager = GameObject.FindWithTag("GameManager");
        gameManager  = manager.GetComponent<AllGameManagement>();
        seManager = manager.GetComponent<AllGameSEManager>();
        sceneChenger = GetComponent<NextScene>();
        inputKeyCheckUI = GameObject.Find("InputImage");
        allSensitivitySlider = GameObject.Find("AllSensitivity").GetComponent<Slider>();
        SensitivityXSlider = GameObject.Find("Sensitivity_X").GetComponent<Slider>();
        SensitivityYSlider = GameObject.Find("Sensitivity_Y").GetComponent<Slider>();
        sellect = KeySellect.None;
        SettingLoader();
    }
    /// <summary> setting load </summary>
    void SettingLoader()
    {
        setFlont    = Controller.Flont;
        setBack     = Controller.Back;
        setRight    = Controller.Right;
        setLeft     = Controller.Left;
        setJump     = Controller.Jump;
        setCrouch   = Controller.Crouch;
        setRun      = Controller.Run;
        setAttack   = Controller.Attack;
        setSkill    = Controller.Skill;
        setUnique   = Controller.Unique;
        setSensiX   = Controller.SensiX;
        setSensiY   = Controller.SensiY;
        setAllSensi = Controller.AllSensitivity;
        SensitivityXSlider.value = setSensiX;
        SensitivityYSlider.value = setSensiY;
        allSensitivitySlider.value = setAllSensi;
    }

    private void Update()
    {
        DownKeyCheck();
        SetSensitivity();
    }

    void SetSensitivity()
    {
        if(setSensiX != SensitivityXSlider.value | setSensiY != SensitivityYSlider.value | setAllSensi != allSensitivitySlider.value)
        {
            setSensiX = SensitivityXSlider.value;
            setSensiY = SensitivityYSlider.value;
            setAllSensi = allSensitivitySlider.value;
            SettingSetter();
        }
    }

    /// <summary>  </summary>
    void DownKeyCheck()
    {
        if (sellect != KeySellect.None)
        {
            inputKeyCheckUI.SetActive(true);
            if (Input.anyKeyDown)
            {
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(code))
                    {
                        if (code == KeyCode.Mouse0 & !resetter) { resetter = true; break; }
                        KeyChenge(code);
                        resetter = false;
                    }
                }
            }
        }
        else inputKeyCheckUI.SetActive(false);
    }

    /// <summary>  </summary>
    /// <param name="keyName"></param>
    public void StartKeyChenge(string keyName)
    {
        switch (keyName)
        {
            case "Flont":  sellect = KeySellect.Flont;  break;
            case "Back":   sellect = KeySellect.Back;   break;
            case "Right":  sellect = KeySellect.Right;  break;
            case "Left":   sellect = KeySellect.Left;   break;
            case "Jump":   sellect = KeySellect.Jump;   break;
            case "Crouch": sellect = KeySellect.Crouch; break;
            case "Run":    sellect = KeySellect.Run;    break;
            case "Attack": sellect = KeySellect.Attack; break;
            case "Skill":  sellect = KeySellect.Skill;  break;
            case "Unique": sellect = KeySellect.Unique; break;
        }
    }
    /// <summary>  </summary>
    void KeyChenge(KeyCode code)
    {
        switch (sellect)
        {
            case KeySellect.Flont:  setFlont  = code; break;
            case KeySellect.Back:   setBack   = code; break;
            case KeySellect.Right:  setRight  = code; break;
            case KeySellect.Left:   setLeft   = code; break;
            case KeySellect.Jump:   setJump   = code; break;
            case KeySellect.Crouch: setCrouch = code; break;
            case KeySellect.Run:    setRun    = code; break;
            case KeySellect.Attack: setAttack = code; break;
            case KeySellect.Skill:  setSkill  = code; break;
            case KeySellect.Unique: setUnique = code; break;
            case KeySellect.None:                     break;
        }
        sellect = KeySellect.None;
        seManager.ButtonSESource.PlayOneShot(seManager.OperationSettingButtonSE[1]);
        SettingSetter();
    }

    /// <summary>  </summary>
    public void GoPreviousScene()
    {
        SettingSetter();
        string goScene;
        switch (gameManager.previousScene)
        {
            case AllGameManagement.Scene.Title:
                goScene = "TitleScene"; break;
            case AllGameManagement.Scene.WeponSellect:
                goScene = "WeponSellectScene"; break;
            case AllGameManagement.Scene.DiceBoad:
                goScene = "DiceBoadScene"; break;
            case AllGameManagement.Scene.Battle:
                goScene = "BattleScene"; break;
            case AllGameManagement.Scene.Result:
                goScene = "ResultScene"; break;
            default:
                goScene = "TitleScene"; break;
        }
        sceneChenger.ChengeScene(goScene);
    }

    /// <summary>  </summary>
    void SettingSetter()
    {
        Controller.Flont    = setFlont;
        Controller.Back     = setBack;
        Controller.Right    = setRight;
        Controller.Left     = setLeft;
        Controller.Jump     = setJump;
        Controller.Crouch   = setCrouch;
        Controller.Run      = setRun;
        Controller.Attack   = setAttack;
        Controller.Skill    = setSkill;
        Controller.Unique   = setUnique;
        Controller.SensiX   = setSensiX;
        Controller.SensiY   = setSensiY;
        Controller.AllSensitivity = setAllSensi;
    }
}

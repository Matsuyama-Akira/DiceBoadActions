using UnityEngine;
using AllGameManager;
using TMPro;

public class ResultUIManager : MonoBehaviour
{
    private AllGameManagement manager;
    private AllGameStates GameStates;
    [SerializeField] TextMeshProUGUI battleResultText;
    int Score;
    string[] Texts = new string[10];

    private void Awake()
    {
        manager = GameObject.FindWithTag("GameManager").GetComponent<AllGameManagement>();
        GameStates = GameObject.FindWithTag("GameManager").GetComponent<AllGameStates>();
    }

    private void Start()
    {
        ResultTextUI();
    }

    void ResultTextUI()
    {
        Score = GameStates.GetKillCount() * GameStates.GetEventMass() * GameStates.GetAllDamage() / GameStates.GetPlayTurn();
        battleResultText.text = "";
        if (manager.result == AllGameManagement.BattleResult.Win) Texts[0] = "Win";
        else Texts[0] = "Lose";
        Texts[1] = "\nenemyKillCount : " + GameStates.GetKillCount().ToString();
        Texts[2] = "\nEventMassCount : " + GameStates.GetEventMass().ToString();
        Texts[3] = "\nPlayTurn : " + GameStates.GetPlayTurn().ToString();
        Texts[4] = "\nAllDamage : " + GameStates.GetAllDamage().ToString();
        Texts[5] = "\nScore : " + Score.ToString();

        int i = 0;
        while (true)
        {
            if (Texts[i] != null)
            {
                battleResultText.text += Texts[i];
            }
            else
            {
                return;
            }
            i++;
        }
    }
}

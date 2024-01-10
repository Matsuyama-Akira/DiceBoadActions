using UnityEngine;
using AllGameManager;
using TMPro;

public class ResultUIManager : MonoBehaviour
{
    // 必須スクリプト
    private AllGameManagement manager;
    private AllGameStates GameStates;

    // 総合スコア
    int Score;

    [SerializeField, Tooltip("リザルトのテキストオブジェクトを入れる")] TextMeshProUGUI battleResultText;
    // テキストとして出力する文字列を入れる
    string[] Texts = new string[10];

    private void Awake()
    {
        // セットアップ
        manager = GameObject.FindWithTag("GameManager").GetComponent<AllGameManagement>();
        GameStates = GameObject.FindWithTag("GameManager").GetComponent<AllGameStates>();

        // テキストの初期化
        battleResultText.text = "";
    }

    private void Start()
    {
        ResultTextUI();
    }

    void ResultTextUI()
    {
        // 総合スコアの計算
        Score = GameStates.GetKillCount() * GameStates.GetEventMass() * GameStates.GetAllDamage() / GameStates.GetPlayTurn();

        // 勝敗判断
        if (manager.result == AllGameManagement.BattleResult.Win) Texts[0] = "Win";
        else Texts[0] = "Lose";

        // 各種スコアを入れる
        Texts[1] = "\nenemyKillCount : " + GameStates.GetKillCount().ToString();
        Texts[2] = "\nEventMassCount : " + GameStates.GetEventMass().ToString();
        Texts[3] = "\nPlayTurn : " + GameStates.GetPlayTurn().ToString();
        Texts[4] = "\nAllDamage : " + GameStates.GetAllDamage().ToString();
        Texts[5] = "\nScore : " + Score.ToString();

        // スコアをテキストに表示
        int i = 0;
        while (true)
        {
            if (Texts[i] == null) return;
            battleResultText.text += Texts[i];
            i++;
        }
    }
}

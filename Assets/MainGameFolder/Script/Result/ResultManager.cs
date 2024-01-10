using UnityEngine;
using NextSceneScript;
using AllGameManager;

public class ResultManager : MonoBehaviour
{
    // 必須スクリプト
    private AllGameStates status;
    private NextScene next;

    private void Awake()
    {
        // セットアップ
        status = GameObject.FindWithTag("GameManager").GetComponent<AllGameStates>();
        next = GetComponent<NextScene>();
    }

    /// <summary>
    /// ゲームのステータスの初期化
    /// </summary>
    public void AllClear()
    {
        // ステータスを初期化
        status.AllReset();

        // すごろくのクリアしたマスを初期化
        int i = 0, j;
        while(i < 11)
        {
            j = 0;
            while(j < 11)
            {
                DiceBoadManagement.clearMass[i, j] = false;
                j++;
            }
            i++;
        }

        // タイトルへ
        next.ChengeScene("TitleScene");
    }
}

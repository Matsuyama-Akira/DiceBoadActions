using UnityEngine;
using NextSceneScript;
using AllGameManager;

public class ResultManager : MonoBehaviour
{
    private AllGameStates status;
    private NextScene next;

    private void Awake()
    {
        status = GameObject.FindWithTag("GameManager").GetComponent<AllGameStates>();
        next = GetComponent<NextScene>();
    }
    public void AllClear()
    {
        status.AllReset();
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
        next.ChengeScene("TitleScene");
    }
}

using UnityEngine;

public class GameEnd : MonoBehaviour
{
    public void gameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        //Game end for editor
#else
        Application.Quit();
        //Game end for application
#endif
    }
}

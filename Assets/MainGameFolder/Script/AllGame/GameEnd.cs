using UnityEngine;

public class GameEnd : MonoBehaviour
{
    public void gameEnd()
    {
#if UNITY_EDITOR
        // エディター上ならプレイモードの終了
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ビルドファイル上ならアプリケーションの終了
        Application.Quit();
#endif
    }
}

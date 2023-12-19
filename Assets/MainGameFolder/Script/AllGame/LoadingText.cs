using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    /// <summary> ロード時間中のフレーム管理 </summary>
    int loadTime;
    [SerializeField, Tooltip("ロード時間のテキスト")] TextMeshProUGUI loadText;

    void Update()
    {
        // ロード中なら
        if (this.gameObject.activeSelf)
        {
            switch (loadTime)
            {
                case 0:
                    loadText.text = "NowLoading";
                    loadTime = 1; break;
                case 1:
                    loadText.text = "NowLoading.";
                    loadTime = 2; break;
                case 2:
                    loadText.text = "NowLoading..";
                    loadTime = 3; break;
                case 3:
                    loadText.text = "NowLoading...";
                    loadTime = 0; break;
            }
        }
    }
}

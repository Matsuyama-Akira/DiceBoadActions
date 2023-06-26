using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    int loadTime;
    [SerializeField] TextMeshProUGUI loadText;

    void Update()
    {
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

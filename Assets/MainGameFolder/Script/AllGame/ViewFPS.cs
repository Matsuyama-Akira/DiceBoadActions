using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewFPS : MonoBehaviour
{
    [SerializeField, Tooltip("フレームレートの処理速度")]
    private float Interval = 0.1f;

    // フレームレートのテキスト
    private TextMeshProUGUI fpsText;

    // 経過時間と処理にかかった時間
    private float elapsedTime, timeCount;
    // 呼び出し回数
    private int frame;
    // 計算したフレームレート
    private float fps;

    private void Start()
    {
        // FPS表示のテキストコンポーネントをロード
        fpsText = this.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        // 経過時間の計測
        elapsedTime -= Time.deltaTime;

        // 前フレームからの経過時間の加算
        timeCount += Time.timeScale / Time.deltaTime;
        // 計測回数の加算
        frame++;

        // 経過時間が0.1秒以上なら
        if (0 >= elapsedTime)
        {
            // フレームレートの計算
            fps = timeCount / frame;

            // 計算結果の表示
            fpsText.text = "FPS: " + fps.ToString("f2");

            // 処理の初期化
            elapsedTime = Interval;
            timeCount = 0;
            frame = 0;
        }
    }
}
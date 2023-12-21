using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AllGameManager;

namespace NextSceneScript
{
    public class NextScene : MonoBehaviour
    {
        /// <summary> 非同期動作で使用するAsyncOperation </summary>
        private AsyncOperation async;
        /// <summary> シーンロード中に表示するUI画面 </summary>
        [SerializeField] private GameObject loadUI;
        /// <summary> 読み込み率を表示するスライダー </summary>
        [SerializeField] private Slider slider;
        /// <summary> シーンデータの送信用 </summary>
        private AllGameManagement manager;

        /// <summary>
        /// データのロード
        /// </summary>
        private void Awake()
        {
            manager = GameObject.FindWithTag("GameManager").GetComponent<AllGameManagement>();
        }

        /// <summary>
        /// 受け取ったシーン名に移行する
        /// </summary>
        /// <param name="SceneName"> 次のシーン名 </param>
        public void ChengeScene(string SceneName)
        {
            // ロード画面UIをアクティブにする
            loadUI.SetActive(true);

            // シーンが切り変わる前のシーン名を送信
            manager.PreviousSceneChenge();

            // コルーチン開始
            StartCoroutine(SceneChenge(SceneName));
        }
        IEnumerator SceneChenge(string SceneName)
        {
            // シーンの読み込みをする
            async = SceneManager.LoadSceneAsync(SceneName);

            // 読み込みが終わるまで進捗状況をスライダーの値に反映させる
            while (!async.isDone)
            {
                var progressVal = Mathf.Clamp01(async.progress / 0.9f);
                slider.value = progressVal;
                yield return null;
            }
        }
    }
}
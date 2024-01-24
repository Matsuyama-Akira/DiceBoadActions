using UnityEngine;
using UnityEngine.SceneManagement;

namespace AllGameManager
{
    public class AllGameManagement : MonoBehaviour
    {
        /// <summary> シーン名 </summary>
        public enum Scene
        {
            Title,
            WeponSellect,
            DiceBoad,
            Battle,
            Result,
            OperationSetting,
        }

        /// <summary> リザルト </summary>
        public enum BattleResult
        {
            Win,
            Lose,
        }

        /// <summary> 現在のシーン名 </summary>
        public Scene nowScene { get; private set; }
        /// <summary> 前回のシーン名 </summary>
        public Scene previousScene { get; private set; }
        /// <summary> 今回のリザルト </summary>
        public BattleResult result { get; private set; }

        /// <summary> BGMデータ </summary>
        private AllGameBGM BGMPleyer;

        /// <summary> FPSの最大値 </summary>
        [SerializeField] private int MaxFrameRate;

        void Awake()
        {
            // このオブジェクトを全てのシーンで引き継ぐ
            DontDestroyOnLoad(this);

            // BGMの取得
            BGMPleyer = GetComponent<AllGameBGM>();
        }
        private void Start()
        {
            // フレームレートを最大30に固定
            Application.targetFrameRate = MaxFrameRate;
        }

        void Update()
        {
            Management();
        }

        /// <summary>
        /// 制御の統括
        /// </summary>
        void Management()
        {
            // 現在のシーンを取得
            SceneChenge();

            // BGMの変更と再生
            BGMPleyer.ChengeBGM((int)nowScene);
        }
        void SceneChenge()
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "TitleScene":             nowScene = Scene.Title;            break;
                case "WeponSellectScene":      nowScene = Scene.WeponSellect;     break;
                case "DiceBoadScene":          nowScene = Scene.DiceBoad;         break;
                case "BattleScene":            nowScene = Scene.Battle;           break;
                case "ResultScene":            nowScene = Scene.Result;           break;
                case "OperationSettingScene":  nowScene = Scene.OperationSetting; break;
            }
        }

        /// <summary>
        /// シーンが切り替わる前のシーン名の取得
        /// </summary>
        public void PreviousSceneChenge()
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "TitleScene":            previousScene = Scene.Title;            break;
                case "WeponSellectScene":     previousScene = Scene.WeponSellect;     break;
                case "DiceBoadScene":         previousScene = Scene.DiceBoad;         break;
                case "BattleScene":           previousScene = Scene.Battle;           break;
                case "ResultScene":           previousScene = Scene.Result;           break;
                case "OperationSettingScene": previousScene = Scene.OperationSetting; break;
            }
        }

        /// <summary>
        /// リザルトの切り替え
        /// </summary>
        /// <param name="_result"> 勝敗結果 0なら勝利 1なら敗北 </param>
        public void AddBattleResult(int _result)
        {
            switch (_result)
            {
                case 0: result = BattleResult.Win;  break;
                case 1: result = BattleResult.Lose; break;
            }
        }
    }
}

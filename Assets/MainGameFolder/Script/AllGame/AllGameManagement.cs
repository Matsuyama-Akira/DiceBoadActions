using UnityEngine;
using UnityEngine.SceneManagement;

namespace AllGameManager
{
    public class AllGameManagement : MonoBehaviour
    {
        public enum Scene
        {
            Title,
            WeponSellect,
            DiceBoad,
            Battle,
            Result,
            OperationSetting,
        }
        public enum BattleResult
        {
            Win,
            Lose,
        }

        public Scene nowScene { get; private set; }
        public Scene previousScene { get; private set; }
        public BattleResult result { get; private set; }

        private AllGameBGM BGMPleyer;

        void Awake()
        {
            DontDestroyOnLoad(this);
            BGMPleyer = GetComponent<AllGameBGM>();
        }
        private void Start()
        {
            Application.targetFrameRate = 30;
            Debug.LogError("FrameRateReset!");
        }

        void Update()
        {
            Management();
        }
        void Management()
        {
            SceneChenge();
            BGMPleyer.ChengeBGM((int)nowScene);
            Debug.LogError(Controller.AllSensitivity + " " + Controller.SensiX + " " + Controller.SensiY);
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

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AllGameManager;

namespace NextSceneScript
{
    public class NextScene : MonoBehaviour
    {
        /// <summary> 次のシーン名 </summary>
        private enum _NextScene
        {
            TitleScene, WeponSellectScene, DiceBoadScene, BattleScene, ResultScene, OperationSettingScene,
        }

        [SerializeField]
        private _NextScene scene;
        //�@�񓯊�����Ŏg�p����AsyncOperation
        private AsyncOperation async;
        //�@�V�[�����[�h���ɕ\������UI���
        [SerializeField]
        private GameObject loadUI;
        //�@�ǂݍ��ݗ���\������X���C�_�[
        [SerializeField]
        private Slider slider;
        private AllGameManagement manager;
        private AllGameStates status;

        private void Awake()
        {
            manager = GameObject.FindWithTag("GameManager").GetComponent<AllGameManagement>();
            status = GameObject.FindWithTag("GameManager").GetComponent<AllGameStates>();
        }

        public void ChengeScene(string SceneName)
        {
            loadUI.SetActive(true);
            manager.PreviousSceneChenge();
            StartCoroutine(SceneChenge(SceneName));
        }
        IEnumerator SceneChenge(string SceneName)
        {
            // �V�[���̓ǂݍ��݂�����
            async = SceneManager.LoadSceneAsync(SceneName);

            //�@�ǂݍ��݂��I���܂Ői���󋵂��X���C�_�[�̒l�ɔ��f������
            while (!async.isDone)
            {
                var progressVal = Mathf.Clamp01(async.progress / 0.9f);
                slider.value = progressVal;
                yield return null;
            }
        }
    }
}
using UnityEngine;

namespace AllGameManager
{
    public class AllGameBGM : MonoBehaviour
    {
        [SerializeField, NamedArray(new string[6] { "Title", "WeponSellect", "DiceBoad", "Battle", "Result", "Operation" })]
        AudioSource[] sceneBGM = new AudioSource[6];
        [SerializeField, NamedArray(new string[3] { "Enemy1", "Enemy2", "Boss" })]
        AudioSource[] battleBGM = new AudioSource[3];
        [SerializeField, NamedArray(new string[2] { "Wins", "Loses" })]
        AudioSource[] ResultBGM = new AudioSource[2];
        AllGameManagement scene;
        EnemySellect enemy;

        private void Awake()
        {
            scene = GetComponent<AllGameManagement>();
            enemy = GetComponent<EnemySellect>();
        }

        public void ChengeBGM(int num)
        {
            if (num == 3) sceneBGM[num] = battleBGM[(int)enemy.level];
            if (num == 4) sceneBGM[num] = ResultBGM[(int)scene.result];
            if (!sceneBGM[num].isPlaying & sceneBGM[num] != null)
            {
                int i = 0;
                while (i < sceneBGM.Length)
                {
                    if (sceneBGM[i] != null & sceneBGM[i].isPlaying) sceneBGM[i].Stop();
                    i++;
                }
                sceneBGM[num].Play();
            }
        }
    }
}

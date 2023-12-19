using UnityEngine;

namespace AllGameManager
{
    public class EnemySellect : MonoBehaviour
    {
        /// <summary> 敵のレベル </summary>
        public enum Level
        {
            Enemy1 = 0,
            Enemy2 = 1,
            Boss = 2,
        }
        /// <summary> 敵のレベル選択 </summary>
        public Level level;

        /// <summary> レベルのセット </summary>
        public void AddLevel(int Lv)
        {
            switch (Lv)
            {
                case 1: level = Level.Enemy1; break;
                case 2: level = Level.Enemy2; break;
                case 3: level = Level.Boss; break;
            }
        }

        /// <summary> レベルの取得 </summary>
        public Level GetLevel() { return level; }
    }

}
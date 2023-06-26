using UnityEngine;

namespace AllGameManager
{
    public class EnemySellect : MonoBehaviour
    {
        public enum Level
        {
            Enemy1 = 0,
            Enemy2 = 1,
            Boss = 2,
        }
        public Level level;
        public void AddLevel(int Lv)
        {
            switch (Lv)
            {
                case 1: level = Level.Enemy1; break;
                case 2: level = Level.Enemy2; break;
                case 3: level = Level.Boss; break;
            }
        }
        public Level GetLevel() { return level; }
    }

}
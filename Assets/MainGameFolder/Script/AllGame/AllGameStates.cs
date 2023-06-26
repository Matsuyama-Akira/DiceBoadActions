using UnityEngine;

namespace AllGameManager
{
    public class AllGameStates : MonoBehaviour
    {
        private WeponSellect wepon;
        [SerializeField, Range(150, 400)] private int startPlayerHP;
        [NamedArray(new string[5] { "Sword", "Spire", "Bow", "Gun", "Magic" }), SerializeField, Range(150, 400)]
        private int[] weponStartPlayerHP = new int[5];
        private int playerHP;

        //Result status
        private int enemyKillCount;
        private int AllDamage;
        private int playTurn;
        private int eventMass;

        //Diceboad status
        private bool start = false;
        private bool lateClear;
        private Vector3 latePosition;
        private Quaternion lateQuaternion;

        private void Awake()
        {
            wepon = GetComponent<WeponSellect>();
        }

        public void AllReset()
        {
            start = false;
            playerHP = weponStartPlayerHP[(int)wepon.wepon];
            enemyKillCount = 0;
            playTurn = 0;
            eventMass = 0;
        }

        public void ResetHP() { playerHP = weponStartPlayerHP[(int)wepon.wepon]; }
        public void AddPlayerHP(int nowHP) { playerHP = nowHP; }
        public void AddHealHP(int heal) { playerHP += heal; if (playerHP >= weponStartPlayerHP[(int)wepon.wepon]) playerHP = weponStartPlayerHP[(int)wepon.wepon]; }
        public void AddKillCount(int kill) { enemyKillCount += kill; }
        public void AddAllDamage(int damage) { AllDamage += damage; }
        public void AddEventMass() { eventMass++; }
        public void AddPlayTurn() { playTurn++; }
        public void AddStart() { start = true; }
        public void AddLateClear(bool isClear) { lateClear = isClear; }
        public void AddPosition(Vector3 latePosi) { latePosition = latePosi; }
        public void AddQuaternion(Quaternion lateQua) { lateQuaternion = lateQua; }

        public int GetWeponHP() { return weponStartPlayerHP[(int)wepon.wepon]; }
        public int GetPlayerHP() { return playerHP; }
        public int GetKillCount() { return enemyKillCount; }
        public int GetAllDamage() { return AllDamage; }
        public int GetPlayTurn() { return playTurn; }
        public int GetEventMass() { return eventMass; }
        public bool GetStart() { return start; }
        public bool GetLateClear() { return lateClear; }
        public Vector3 GetLatePosition() { return latePosition; }
        public Quaternion GetLateQuaternion() { return lateQuaternion; }
    }
}

using UnityEngine;

namespace AllGameManager
{
    public class AllGameStates : MonoBehaviour
    {
        // 必須スクリプト
        private WeponSellect wepon;

        [SerializeField, Range(150, 400), Tooltip("ゲームスタート時のプレイヤーのHP")] private int startPlayerHP;
        [NamedArray(new string[5] { "Sword", "Spear", "Bow", "Gun", "Magic" }), SerializeField, Range(150, 400), Tooltip("武器毎のプレイヤーの最大HP")]
        private int[] weponPlayerHP = new int[5];
        /// <summary> 現在のプレイヤーのHP </summary>
        private int playerHP;
        /// <summary> ダメージを受ける前のプレイヤーのHP </summary>
        private int playerLateHP;

        // リザルト用の数値
        /// <summary> 倒した敵の数 </summary>
        private int enemyKillCount;
        /// <summary> 与えたダメージの総数 </summary>
        private int AllDamage;
        /// <summary> すごろくでサイコロを振った数 </summary>
        private int playTurn;
        /// <summary> すごろくでイベントマスに止まった数 </summary>
        private int eventMass;

        // すごろく用のステータス
        /// <summary> ゲームをスタートしたか </summary>
        private bool start = false;
        /// <summary> マスをクリアしたか </summary>
        private bool lateClear;
        /// <summary> シーンが切り替わる前の位置 </summary>
        private Vector3 latePosition;
        /// <summary> シーンが切り替わる前の回転 </summary>
        private Quaternion lateQuaternion;

        private void Awake()
        {
            // セットアップ
            wepon = GetComponent<WeponSellect>();
        }

        /// <summary>
        /// ゲームのステータスのリセット
        /// </summary>
        public void AllReset()
        {
            start = false;
            playerHP = weponPlayerHP[(int)wepon.wepon];
            enemyKillCount = 0;
            playTurn = 0;
            eventMass = 0;
        }

        // 各データの挿入
        /// <summary> プレイヤーのHPをリセット </summary>
        public void ResetHP() { playerHP = weponPlayerHP[(int)wepon.wepon]; }
        /// <summary> プレイヤーの現在のHPをセット </summary>
        public void AddPlayerHP(int nowHP) { playerHP = nowHP; }
        /// <summary> プレイヤーがダメージを受ける前のHPをセット </summary>
        public void AddPlayerLateHP() { playerLateHP = playerHP; }
        /// <summary> プレイヤーのHPをhealの分増加。武器毎の最大HPを超えていたら最大HPに </summary>
        public void AddHealHP(int heal) { playerHP += heal; if (playerHP >= weponPlayerHP[(int)wepon.wepon]) playerHP = weponPlayerHP[(int)wepon.wepon]; }
        /// <summary> 敵を倒した数を増加 </summary>
        public void AddKillCount(int kill) { enemyKillCount += kill; }
        /// <summary> ダメージを与えた総数を増加 </summary>
        public void AddAllDamage(int damage) { AllDamage += damage; }
        /// <summary> イベントマスに止まった数を増加 </summary>
        public void AddEventMass() { eventMass++; }
        /// <summary> サイコロを振った数を増加 </summary>
        public void AddPlayTurn() { playTurn++; }
        /// <summary> ゲームをスタート </summary>
        public void AddStart() { start = true; }
        /// <summary> 現在のマスをクリア </summary>
        public void AddLateClear(bool isClear) { lateClear = isClear; }
        /// <summary> 現在のマスの位置をセット </summary>
        public void AddPosition(Vector3 latePosi) { latePosition = latePosi; }
        /// <summary> 現在のプレイヤーの回転をセット </summary>
        public void AddQuaternion(Quaternion lateQua) { lateQuaternion = lateQua; }

        // 各データの送信
        /// <summary> 武器毎の最大HP </summary>
        public int GetWeponHP() { return weponPlayerHP[(int)wepon.wepon]; }
        /// <summary> プレイヤーの現在のHP </summary>
        public int GetPlayerHP() { return playerHP; }
        /// <summary> プレイヤーがダメージを受ける前のHP </summary>
        public int GetLateHP() { return playerLateHP; }
        /// <summary> 敵を倒した数 </summary>
        public int GetKillCount() { return enemyKillCount; }
        /// <summary> 与えたダメージの総数 </summary>
        public int GetAllDamage() { return AllDamage; }
        /// <summary> サイコロを振った数 </summary>
        public int GetPlayTurn() { return playTurn; }
        /// <summary> イベントマスに止まった数 </summary>
        public int GetEventMass() { return eventMass; }
        /// <summary> スタートしているか </summary>
        public bool GetStart() { return start; }
        /// <summary> クリア状態 </summary>
        public bool GetLateClear() { return lateClear; }
        /// <summary> シーンが切り替わる前の位置 </summary>
        public Vector3 GetLatePosition() { return latePosition; }
        /// <summary> シーンが切り替わる前の回転 </summary>
        public Quaternion GetLateQuaternion() { return lateQuaternion; }
    }
}

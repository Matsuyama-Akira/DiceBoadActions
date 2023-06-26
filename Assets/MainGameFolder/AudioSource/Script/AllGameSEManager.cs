using UnityEngine;

namespace AllGameManager
{
    public class AllGameSEManager : MonoBehaviour
    {
        [Header("AudioSource")]

        [Header(">AllGame")]
        public AudioSource ButtonSESource;

        [Header(">DiceBoad")]
        public AudioSource DB_MassSESource;
        public AudioSource DB_FootSESource;

        [Header(">Battle")]
        public AudioSource BT_GameSESource;


        [Header("SE Clip")]

        [Header(">Title")]
        [NamedArray(new string[] {"GoWS", "GoOS"})]
        public AudioClip[] TitleButtonSE;

        [Header(">WeponSellect")]
        [NamedArray(new string[] {"WeponSellect", "NextScene"})]
        public AudioClip[] WeponSellectButtonSE;

        [Header(">DiceBoad")]
        [NamedArray(new string[] {"WeponChengeButton", "DiceRole"})]
        public AudioClip[] DB_AnySE;
        [NamedArray(new string[] {"Normal", "Items", "Heal", "Enemy"})]
        public AudioClip[] DB_MassSE;
        public AudioClip[] DB_FootSE;

        [Header(">Battle")]
        public AudioClip[] BattleButtonSE;
        public AudioClip[] BT_FootSE;
        [NamedArray(new string[] {"Charge", "Shoot", "Hit"})]
        public AudioClip[] BT_BowAttackSE;
        [NamedArray(new string[] {"Attack1", "Attack2", "Guard", "Hit"})]
        public AudioClip[] BT_SwordAttackSE;
        [NamedArray(new string[] {"Attack", "Hit"})]
        public AudioClip[] BT_EnemyAttackSE;

        [Header(">Result")]
        [NamedArray(new string[] {"GoTitle"})]
        public AudioClip[] ResultButtonSE;

        [Header(">OperationSetting")]
        [NamedArray(new string[] {"KeyInput", "SetKey", "BackScene"})]
        public AudioClip[] OperationSettingButtonSE;
    }
}

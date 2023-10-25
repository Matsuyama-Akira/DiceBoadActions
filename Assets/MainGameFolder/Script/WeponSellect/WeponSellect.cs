using UnityEngine;

namespace AllGameManager
{
    public class WeponSellect : MonoBehaviour
    {
        public enum Wepon
        {
            Sword   = 0,
            Spear   = 1,
            Bow     = 2,
            Gun     = 3,
            Magic   = 4,
        }
        public enum Rarelity
        {
            Common  = 0,
            Rare    = 1,
            Unique  = 2,
        }

        public Wepon wepon;
        public Rarelity rarelity;

        public void weponSellecting(string weponSellect)
        {
            switch (weponSellect)
            {
                case "Sword":       wepon = Wepon.Sword; break;
                case "Spire":       wepon = Wepon.Spear; break;
                case "Bow":         wepon = Wepon.Bow;   break;
                case "Gun":         wepon = Wepon.Gun;   break;
                case "MagicWepon":  wepon = Wepon.Magic; break;
            }
        }
        public void SetRarelity(int level)
        {
            switch (level)
            {
                case 0: rarelity = Rarelity.Common; break;
                case 1: rarelity = Rarelity.Rare;   break;
                case 2: rarelity = Rarelity.Unique; break;
            }
        }
    }
}

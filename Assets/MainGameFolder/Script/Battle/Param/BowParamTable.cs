using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BowParamTable", menuName = "CreateParamData/CreateBowParam")]
public class BowParamTable : ScriptableObject
{
    public List<BowSellection> BowList = new List<BowSellection>();
}

[System.Serializable]
public class BowSellection
{
    public enum Level
    {
        Common  = 0,
        Rare    = 1,
        Unique  = 2,
    }
    public enum BowType
    {
        Noumal,
        Penetrate,
        Diffusion
    }
    public Level level;
    public BowType type;
    [Range(1, 100)] public int baseDamage;
    [Range(1.0f, 2.0f)] public float magunification;
    [Range(0, 100)] public int criticalRange;
}

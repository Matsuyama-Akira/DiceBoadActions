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
    /// <summary> 武器のレベル </summary>
    public enum Level
    {
        Common  = 0,
        Rare    = 1,
        Unique  = 2,
    }

    /// <summary> 矢の属性 </summary>
    public enum BowType
    {
        Noumal,
        Penetrate,
        Diffusion
    }

    /// <summary> この武器のレベル選択 </summary>
    public Level level;

    /// <summary>  </summary>
    public BowType type;

    /// <summary>  </summary>
    [Range(1, 100)] public int baseDamage;

    /// <summary>  </summary>
    [Range(1.0f, 2.0f)] public float magunification;

    /// <summary>  </summary>
    [Range(0, 100)] public int criticalRange;
}

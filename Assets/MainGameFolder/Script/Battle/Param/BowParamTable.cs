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

    /// <summary> この武器の矢の属性 </summary>
    public BowType type;

    /// <summary> 基礎ダメージ </summary>
    [Range(1, 100)] public int baseDamage;

    /// <summary> チャージ量倍率の最大値 </summary>
    [Range(1.0f, 2.0f)] public float magunification;

    /// <summary> クリティカル確率 </summary>
    [Range(0, 100)] public int criticalRange;
}

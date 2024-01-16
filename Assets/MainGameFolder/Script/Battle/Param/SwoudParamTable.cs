using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordParamTable", menuName = "CreateParamData/CreateSwordParam")]
public class SwoudParamTable : ScriptableObject
{
    public List<SwordSellection> SwordList = new List<SwordSellection>();
}

[System.Serializable]
public class SwordSellection
{
    /// <summary> 武器のレアリティ </summary>
    public enum Level { Common, Rare, Unique, }

    /// <summary> この武器のレアリティ </summary>
    public Level level;
    /// <summary> 基礎ダメージ </summary>
    [Range(1, 200)] public int baseDamage;
    /// <summary> クリティカル確率 </summary>
    [Range(0, 100)] public int criticalRange;
}

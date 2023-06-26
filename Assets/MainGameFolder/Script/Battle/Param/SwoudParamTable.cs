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
    public enum Level { Common, Rare, Unique, }
    public Level level;
    [Range(1, 200)] public int baseDamage;
    [Range(0, 100)] public int criticalRange;
}

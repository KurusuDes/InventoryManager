using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatiscsType
{
    None,
    Damage,
    Health,
    Speed,
    Defense
}

public enum ModifierType
{
    None,
    Fire,
    Air,
    Poison,
    Earth,
    Ice,
    Electric
}

[Serializable]
public struct StatModifier
{
    public StatiscsType Statistic;
    public int Value;
}



[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemSO : BaseEntitySO
{
    #region References 
    [ShowInInspector, InlineEditor] public EffectDirection chainEffect;
    [ShowInInspector] public List<StatModifier> Stats;


    #endregion


}



using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum EffectType
{
    None,
    Add,
    Substract,
    Multiply,
    Divide,

}
[Serializable]
public struct EffectRange
{
    public Directions direction;
    public int step;
    public ValueModifier valueModifier;

}
[Serializable]
public struct ValueModifier
{
    public EffectType effectType;
    public float modifierValue;

}

[CreateAssetMenu(fileName = "EffectDirection", menuName = "ScriptableObjects/EffectDirection", order = 1)]
[ShowInInspector]
public class EffectDirection : SerializedScriptableObject
{

    [SerializeField] public List<EffectRange> rangeOfEffects = new List<EffectRange>();

}

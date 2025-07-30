using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;


/*public interface IEffect
{
    void ApplyEffect();
}
*/
public class Item 
{
    private List<ValueModifier> modifiers = new List<ValueModifier>();

    public Slot<Item> ItemSlot;
    [ShowInInlineEditors]public ItemSO Value ;

    public List<StatModifier> currentStat;

    public Item(ItemSO value)
    {
        Value = value;
        currentStat = new List<StatModifier>(Value.Modifier);
    }
    public Item(ItemSO value , Slot<Item> slot)
    {
        Value = value;
        ItemSlot = slot;
        currentStat = new List<StatModifier>(Value.Modifier);
    }
    void Start()
    {
        ResetStats();
    }
    public void ResetStats()//->Cuando se mueve algo limpiar los modifiers
    {
        currentStat.Clear();
        currentStat.AddRange(Value.Modifier);
        // currentStat = new List<StatModifier>(Value.Modifier); 
    }
    public void AddModifier(ValueModifier effect)
    {
        modifiers.Add(effect);
       /* if (effect != null && !modifiers.Contains(effect))
        {
            modifiers.Add(effect);
        }*/
    }
    public void RemoveModifier(ValueModifier effect)
    {
        modifiers.Remove(effect);
        /*
        if (effect != null && modifiers.Contains(effect))
        {
            modifiers.Remove(effect);
        }*/
    }

    public void ApplyEffect()
    {
        SortValueModifiers(modifiers);

        for (int i = 0; i < currentStat.Count; i++)
        {
            float value = currentStat[i].Value;

            foreach (var mod in modifiers)
            {
                switch (mod.effectType)
                {
                    case EffectType.Add:
                        value += mod.modifierValue;
                        break;
                    case EffectType.Substract:
                        value -= mod.modifierValue;
                        break;
                    case EffectType.Divide:
                        value /= mod.modifierValue;
                        break;
                    case EffectType.Multiply:
                        value *= mod.modifierValue;
                        break;
                }
            }

            // Asignar el valor modificado de vuelta
            StatModifier stat = currentStat[i];
            stat.Value = (int)value;
            currentStat[i] = stat;

        }
    }


    public void SortValueModifiers(List<ValueModifier> modifiers)
    {
        modifiers.Sort((a, b) =>
            GetEffectPriority(a.effectType).CompareTo(GetEffectPriority(b.effectType)));
    }
    private int GetEffectPriority(EffectType type)
    {
        return type switch
        {
            EffectType.Add => 0,
            EffectType.Substract => 0,
            EffectType.Divide => 1,
            EffectType.Multiply => 2,
            _ => 3 // None u otros
        };
    }
   
    [Button]
    public void ApplyModifiers()
    {
        if (Value.chainEffect == null || Value.chainEffect.rangeOfEffects.Count == 0)
        {
            Debug.LogWarning("No effects to apply.");
            return;
        }

        List<EffectRange> rangeOfEffects = Value.chainEffect.rangeOfEffects;

        for (int i = 0; i < rangeOfEffects.Count; i++)
        {
            Debug.Log("TryToApplyEffects");
            ApplyModifier(rangeOfEffects[i].direction, rangeOfEffects[i].valueModifier, ItemSlot, rangeOfEffects[i].step);//-> pasa y aplica pasa y aplica
        }
    }
    public void ApplyModifier(Directions direction,ValueModifier valueModifier , Slot<Item> target , int step  = 0)//- que pase el tipo de effecto tmb
    {
        if (step == 0) return;
       
        if (target.GetNeighbor(direction, out Slot<Item> neighborSlot))
        {
            Debug.Log("TryToApplyEffect");


            Item neighborItem = neighborSlot.Value;

            if (neighborItem != null)
            {
                Debug.Log("Succes");
                neighborItem.AddModifier(valueModifier);
            }

            ApplyModifier(direction, valueModifier, neighborSlot, step - 1);
        }
        else
            return;

    }
    public void ShowModifiers()
    {
        if(modifiers.Count == 0)
        {
            Debug.Log("No modifiers applied.");
            return;
        }   
        foreach (var modifier in modifiers)
        {
            Debug.Log($"Modifier: {modifier.modifierValue}, Value: {modifier.effectType.ToString()}");
        }
    }
}

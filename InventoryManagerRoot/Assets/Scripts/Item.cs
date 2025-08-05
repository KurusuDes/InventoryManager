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

    public Slot parentSlot;
    [ShowInInlineEditors]public ItemSO itemSO ;

    public List<StatModifier> BaseStats = new();
    public List<StatModifier> ModifiedStats = new();

    public void SetValues(ItemSO value)
    {
        itemSO = value;
        BaseStats = new List<StatModifier>(itemSO.Stats);
        ModifiedStats = new List<StatModifier>(BaseStats);
    }
    public bool HasItem()
    {
        return itemSO != null;
    }
    public bool CompareItem(ItemSO _value)
    {
        return itemSO.Equals(_value);
    }
    public void ClearItem()//->Cuando se mueve algo limpiar los modifiers
    {
        itemSO = null;
        ResetStats();

    }
    public void ResetStats()//->Cuando se mueve algo limpiar los modifiers
    {
        modifiers.Clear();
        ModifiedStats.Clear();
        ModifiedStats = new List<StatModifier>(BaseStats);

    }
    public void AddModifier(ValueModifier effect)
    {
        modifiers.Add(effect);
    }
    public void RemoveModifier(ValueModifier effect)
    {
        modifiers.Remove(effect);
    }

    public void ApplyEffect()
    {
        SortValueModifiers(modifiers);

        for (int i = 0; i < ModifiedStats.Count; i++)
        {
            float value = ModifiedStats[i].Value;

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
            StatModifier stat = ModifiedStats[i];
            stat.Value = (int)value;
            ModifiedStats[i] = stat;

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
    public void ApplyModifiers()//-> LOS SLOTS VACIOS NO DEBEN SIGNIFICAR QUE TE DETIENE LOS SLOTS VACIOS DEBEN CONTENER LOS MODIFICADORES

    {
        //modifiers.Clear();//->RESETEAR MODIFICADORES

        if (itemSO == null||itemSO.chainEffect == null || itemSO.chainEffect.rangeOfEffects.Count == 0)
        {
            //Debug.LogWarning("No effects to apply.");
            return;
        }

        List<EffectRange> rangeOfEffects = itemSO.chainEffect.rangeOfEffects;

        for (int i = 0; i < rangeOfEffects.Count; i++)
        {
            //Debug.Log("TryToApplyEffects");
            if(parentSlot == null)
            {
                throw new System.Exception("Parent slot is null, cannot apply effects.");
            }
            ApplyModifier(rangeOfEffects[i].direction, rangeOfEffects[i].valueModifier, parentSlot, rangeOfEffects[i].step);//-> pasa y aplica pasa y aplica
        }
    }
    public void ApplyModifier(Directions direction,ValueModifier valueModifier , Slot target , int step  = 0)//- que pase el tipo de effecto tmb
    {
        if (step == 0)
        {
            //Debug.Log("Step is 0, stopping recursion.");
            return;
        }
       
        if (target.GetNeighbor(direction, out Slot neighborSlot))
        {
           // Debug.Log("TryToApplyEffect");


            Item neighborItem = neighborSlot.Item;

            if (neighborItem != null)
            {
               // Debug.Log("Succes");
                neighborItem.AddModifier(valueModifier);
            }

            ApplyModifier(direction, valueModifier, neighborSlot, step - 1);
        }
       /* else
            return;*/

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



    public List<ValueModifier> Modifiers => modifiers;
}

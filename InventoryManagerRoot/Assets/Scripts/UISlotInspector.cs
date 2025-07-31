
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UISlotInspector : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI textPrefab;
    public RectTransform container;

    public List<TextMeshProUGUI> txts;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetSlot(Item slot)
    {



        icon.sprite = null;

        foreach (var item in txts)
        {
            Destroy(item.gameObject);
        }
        txts.Clear();
        if (slot == null)
            return;



        icon.sprite = slot.itemSO.Icon;

        foreach (var stat in slot.BaseStats)
        {
            TextMeshProUGUI txt =  Instantiate(textPrefab, container);
            txt.text = stat.Statistic.ToString()+ " : " + stat.Value;
            txts.Add(txt);  
        }
        foreach (var stat in slot.ModifiedStats)
        {
            TextMeshProUGUI txt = Instantiate(textPrefab, container);
            txt.text = stat.Statistic.ToString() + " : " + stat.Value;
            txts.Add(txt);
        }
        foreach (var modifier in slot.Modifiers)
        {
            TextMeshProUGUI txt = Instantiate(textPrefab, container);
            txt.text = modifier.effectType.ToString() + " == " + modifier.modifierValue;
            txts.Add(txt);
        }
        if(slot.itemSO.chainEffect  != null)
        {
            foreach (var effects in slot.itemSO.chainEffect.rangeOfEffects)
            {
                TextMeshProUGUI txt = Instantiate(textPrefab, container);
                txt.text = effects.direction.ToString() + " ; " + effects.step + " ; " + effects.valueModifier.effectType + ",";
                txts.Add(txt);
            }
        }
        
    }
}

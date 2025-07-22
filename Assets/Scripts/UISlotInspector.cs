
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

        icon.sprite =  slot.Value.Icon;



        foreach (var item in txts)
        {
            Destroy(item.gameObject);
        }
        txts.Clear();

        foreach (var stat in slot.currentStat)
        {
           TextMeshProUGUI txt =  Instantiate(textPrefab, container);
            txt.text = stat.Statistic.ToString()+ " : " + stat.Value;
            txts.Add(txt);  
        }
    }
}

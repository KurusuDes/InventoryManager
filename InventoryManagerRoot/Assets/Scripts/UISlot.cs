using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;
using NUnit.Framework.Interfaces;

[Serializable]
public struct UINeighbor
{
    public Directions Direction;
    public GameObject Connector;
    public UINeighbor(Directions direction,GameObject _image)
    {
        Connector = _image;
        Direction = direction;
    }
}

public class UISlot : MonoBehaviour//->MOSTRAR LOS STATS MODIFICADOS Y ACTUALIZARLOS
{
    #region References
    public GameObject LineRendererPrefab;


    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI amountTxt;
    public Image icon;
    public Image background;
    public Sprite EnableConnector;
    public Sprite DisableConnector;

    public Color normalColor;
    public Color highlightColor;
    #endregion
    [SerializeField]private int itemDatabaseID = -1;
    [SerializeField]private int uiSlotIndex =-1;
    private bool selected = false;
    public UIInventory uiInvetory;
    public List<UINeighbor> Neighbors;




    public Item ItemData;

    //public GameObject SampleTextValue;



    public int UISlotIndex => uiSlotIndex;
    public int ItemDatabaseID => itemDatabaseID;
    public bool HasContent = false;
    void Start()
    {
        HasContent = false;
        
    }
    public void SetNeighbors(Slot _slot, UIInventory _uiIventory)
    {
        foreach (var neighbor in Neighbors)
        {
            if (!_slot.Neighbors.TryGetValue(neighbor.Direction, out Slot value))
            {
                neighbor.Connector.SetActive(false);
            }
        }

    }
    public void SetSlot(Slot _slot , UIInventory _uiIventory)
    {      
        if (  _slot == null || _slot.Item.itemSO == null || _slot.Item == null  )
        {
            Clear();
            
            return;
        }

        if (_slot.IsBlocked)
        {
            nameTxt.text = "Blocked";
            amountTxt.text = "";
            icon.sprite = null;
            return;
        }

       // print("El nombre del tempSlot" + _slot.itemSO.EntityName);

        itemDatabaseID = _slot.Item.itemSO.ID;

        nameTxt.text = _slot.Item.itemSO.EntityName;
        amountTxt.text = _slot.Quantity.ToString();
        icon.sprite = _slot.Item.itemSO.Icon;


        uiInvetory = _uiIventory;
        HasContent = true;


        ItemData = _slot.Item;

        SetUIConnectors();

    }
    public void SetUIConnectors()
    {
        foreach (var effects in ItemData.itemSO.chainEffect.rangeOfEffects)
        {


            foreach (var neighbor in Neighbors)
            {
                if (effects.direction == neighbor.Direction)
                {
                    neighbor.Connector.GetComponent<Image>().sprite = EnableConnector;
                }
                else
                {
                    neighbor.Connector.GetComponent<Image>().sprite = DisableConnector;
                }
            }
           // if (Neighbors.   effects.direction)

           
        }
    }

    public void Set(int id)
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null.");
            return;
        }
        ItemSO item = GameManager.Instance.GetItemByID(id);
        if (item == null)
        {
            Clear();
            return;
        }
        nameTxt.text = item.EntityName;
        amountTxt.text = ""; // Assuming quantity is always 1 for this example
        icon.sprite = item.Icon;
    }
    public void SetSlot(int _uiSlotIndex)
    {
        uiSlotIndex = _uiSlotIndex;

    }
    public void Clear()
    {
        ItemData = null;
        nameTxt.text = "";
        amountTxt.text = "";
        icon.sprite = null;
        itemDatabaseID = -1;
        HasContent = false;
    }
    public ItemSO GetItem()
    {
        return GameManager.Instance.GetItemByID(itemDatabaseID);
    }
   /* public Slot<ItemSO> GetSlot()
    {
        return null;
    }*/
    private void Highlight(bool _highlight)
    {
        print("Try To Change Color");
        background.color = _highlight ? highlightColor :  normalColor;

    }
    public void SwitchSelectedState()//generar un conflicto cuando se quiran mostrar varios paths highlighted en un futuro actualizar
    {
        selected = !selected;

        Highlight(selected);
    }
    public void SwitchSelectedState(bool state)//generar un conflicto cuando se quiran mostrar varios paths highlighted en un futuro actualizar
    {
        selected = state;

        Highlight(selected);
    }

}

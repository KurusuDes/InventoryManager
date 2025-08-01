using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;
using System;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    #region References
    public UIInventory uiInventory;
    public UISlotInspector uiSlotInspector;
    #endregion
    #region Settings
    public int size = 9;
    public int rows = 3;//rows
    #endregion
    #region Variables 
    [ShowInInlineEditors]public SlotManager<Item> ItemManager;
    #endregion
    #region Actions
   
    #endregion
    void Start()
    {
       SetInventory();
    }
    public void SetInventory()
    {
        ItemManager = new(size);
        ItemManager.SetNeighbors(rows: 3);

        ItemManager.OnChange += SetAndTriggerEffects;
        SetUIInventory();
    }
    public void SetAndTriggerEffects()
    {
       ClearAllEffects();
       SetAllEffects();
       EnableAllEffects();
    }
    public void SetUIInventory()
    {
        uiInventory.SetInventory(this);
        ItemManager.OnSlotAdded += uiInventory.SetSlot;
        ItemManager.OnSlotUpdated += uiInventory.SetSlot;
        ItemManager.OnSlotCleared += uiInventory.ClearSlot;
     
    }
    [Button]
    public void AddItemToInventory(int _position, ItemSO _itemSO, int _quantity = 0)
        => ItemManager.Add(_position, _itemSO, _quantity);

    #region Helpers
    [Button]
    public void SwapItems(int fromIndex, int toIndex) 
        => ItemManager.Swap(fromIndex, toIndex);

    #endregion
    #region Debug
    
    [Button]
    public void FillWithRandomItems()
    {
        for (int i = 0; i < ItemManager.CurrentSlots.Count; i++)
        {
            int randomID = UnityEngine.Random.Range(0, GameManager.Instance.itemDatabase.GetCount());

            ItemSO itemSOref = GameManager.Instance.itemDatabase.ItemsList[randomID];


            /*
            Item item = new Item(GameManager.Instance.itemDatabase.ItemsList[randomID], ItemManager.CurrentSlots[i]);
            print(ItemManager.CurrentSlots[i]);*/


            AddItemToInventory(i, itemSOref, UnityEngine.Random.Range(1, 10));
        }
    }
    [Button]
    public void PrintNeighbors(int position)
    {
        foreach (var slot in ItemManager.CurrentSlots[position].Neighbors)
        {
            print(slot.Key);
            print(slot.Value.Item.itemSO.EntityName);
           
        }
    }
    [Button]
    public void SetAllEffects()//->Disparar todos los efecto y activarlos
    {
        for (int i = 0; i < ItemManager.CurrentSlots.Count; i++)
        {
            //print(ItemManager.CurrentSlots[i].Item.parentSlot);
            if (ItemManager.CurrentSlots[i].Item == null) continue;

            ItemManager.CurrentSlots[i].Item.ApplyModifiers();
        }
       
    }
    [Button]
    public void ClearAllEffects()//->Limpia
    {
        //print(ItemManager.CurrentSlots.Count);
        for (int i = 0; i < ItemManager.CurrentSlots.Count; i++)
        {
           // if (ItemManager.CurrentSlots[i].itemSO == null) continue;

            ItemManager.CurrentSlots[i].Item.ResetStats();
        }

    }
    [Button]
    public void EnableAllEffects()//->Disparar todos los efecto y activarlos
    {
        for (int i = 0; i < ItemManager.CurrentSlots.Count; i++)
        {
            if (ItemManager.CurrentSlots[i].Item == null) continue;

            ItemManager.CurrentSlots[i].Item.ApplyEffect();
        }

    }
    [Button]
    public void SetEffect(int position)
    {
        if (ItemManager.CurrentSlots[position].Item == null) return;
        ItemManager.CurrentSlots[position].Item.ApplyModifiers();  
    }
    [Button]
    public void EnableEffects(int position)
    {
        if (ItemManager.CurrentSlots[position].Item == null) return;
        ItemManager.CurrentSlots[position].Item.ApplyEffect();
    }
    #endregion


}
/*->TO DO
 - Por alguna razon no se actualiza el valor principal
*/
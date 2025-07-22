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
        ItemManager = new(null, size);
        ItemManager.SetNeighbors(rows: 3);

        SetUIInventory();
    }
    public void SetUIInventory()
    {
        uiInventory.SetInventory(this);
        ItemManager.OnSlotAdded += uiInventory.SetSlot;
        ItemManager.OnSlotUpdated += uiInventory.SetSlot;
        ItemManager.OnSlotCleared += uiInventory.ClearSlot;
    }
    [Button]
    public void AddItemToInventory(int _position, Item _item, int _quantity = 0)
        => ItemManager.Add(_position, _item, _quantity);

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

            Item item = new Item(GameManager.Instance.itemDatabase.ItemsList[randomID], ItemManager.CurrentSlots[i]);


            AddItemToInventory(i, item, UnityEngine.Random.Range(1, 10));
        }
    }
    [Button]
    public void PrintNeighbors(int position)
    {
        foreach (var slot in ItemManager.CurrentSlots[position].Neighbors)
        {
            print(slot.Key);
            print(slot.Value.Value.Value.EntityName);
           
        }
    }
    [Button]
    public void SetAllEffects()//->Disparar todos los efecto y activarlos
    {
        for (int i = 0; i < ItemManager.CurrentSlots.Count; i++)
        {
            ItemManager.CurrentSlots[i].Value.ApplyModifiers();
        }
       
    }
    [Button]
    public void EnableAllEffects()//->Disparar todos los efecto y activarlos
    {
        for (int i = 0; i < ItemManager.CurrentSlots.Count; i++)
        {
            ItemManager.CurrentSlots[i].Value.ApplyEffect();
        }

    }
    [Button]
    public void ApplyEffectOfItem(int position)
    {
         ItemManager.CurrentSlots[position].Value.ApplyModifiers();  
    }
    [Button]
    public void ShowModifiers(int position)
    {
        ItemManager.CurrentSlots[position].Value.ShowModifiers();
    }
    #endregion


}

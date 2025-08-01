using System;
using System.Collections.Generic;
using UnityEngine;
public enum Directions
{
    None,
    North, //Arriba
    South,//Abajo
    East,//Derecha
    West,//Izquierda
    NorthEast,//Arriba derecha
    NorthWest,//Arriba izquierda
    SouthEast,//Abajo derecha
    SouthWest//Abajo izquierda
}

public class Slot
{
    private Item item = new();
    private int quantity = 0;
    private bool isBlocked = false;
    private bool isHighlighted = false;
    
    private Dictionary<Directions, Slot> neighbors = new Dictionary<Directions, Slot>();

    public Item Item => item;
    public int Quantity => quantity;
    public bool IsBlocked => isBlocked; 
    public bool IsHighlighted => isHighlighted;

    public ItemSO ItemSO => item.itemSO;
    public Slot()
    {
        item = new();
        item.parentSlot = this;
        quantity = 0;
    }
    public Slot(int _quantity)
    {
        item = new();
        item.parentSlot = this;
        quantity = _quantity;
    }
    public Slot(bool _isBlocked)
    {
        isBlocked = _isBlocked;
    }
    public void SetNeighbor(Directions _direction , Slot slot)
    {
        //neighbors[_direction] = slot;
        neighbors.Add(_direction, slot);
    }
    public bool GetNeighbor(Directions _direction , out Slot slot)
    {
        if (neighbors.TryGetValue(_direction, out var neighbor) && neighbor != null)
        {
            slot = neighbor;
            return true;
        }

        slot = null;
        return false;
    }
    public void AddQuantity(int _quantity)
    {
        quantity += _quantity;
    }
    public bool CompareSlot(ItemSO itemSO)
    {
        if(itemSO == null)  return false;

        return item.CompareItem(itemSO);
    }
    public void SetValue(ItemSO _value)
    {
        item.SetValues(_value);
    }
    public void Clear()
    {
        item.ClearItem();
        quantity = 0;
        isBlocked = false;
        isHighlighted = false;
    }
    public void ClearDirections()
    {
        neighbors.Clear();
        //neighbors[Directions.West] = null;
    }
    public void RemoveDirection(Directions dir)
    {
        neighbors[dir] = null;
    }
    /*public Slot Clone()
    {
        Slot clone = new Slot(item, quantity);
        
        return clone;
    }*/

    public Dictionary<Directions, Slot> Neighbors => neighbors;
}

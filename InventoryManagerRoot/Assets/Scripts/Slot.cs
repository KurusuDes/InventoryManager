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
    private Item value;
    private int quantity = 0;
    private bool isBlocked = false;
    private bool isHighlighted = false;
    
    private Dictionary<Directions, Slot> neighbors = new Dictionary<Directions, Slot>();

    public Item Value => value;
    public int Quantity => quantity;
    public bool IsBlocked => isBlocked; 
    public bool IsHighlighted => isHighlighted;

    public Slot(Item _value)
    {
        value = _value;
    }
    public Slot(Item _value, int _quantity)
    {
        value = _value;
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
    public bool CompareSlot(Item _value)
    {
        if(_value == null)
        {
            return false;
        }

        return value.CompareItem(_value.Value);
    }
    public void SetValue(Item _value)
    {
        value = _value;
    }
    public void Clear()
    {
        value = default;
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
    public Slot Clone()
    {
        Slot clone = new Slot(value, quantity);
        
        return clone;
    }

    public Dictionary<Directions, Slot> Neighbors => neighbors;
}

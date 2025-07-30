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

public class Slot<T>
{
    private T value;
    private int quantity = 0;
    private bool isBlocked = false;
    private bool isHighlighted = false;
    
    private Dictionary<Directions, Slot<T>> neighbors = new Dictionary<Directions, Slot<T>>();

    public T Value => value;
    public int Quantity => quantity;
    public bool IsBlocked => isBlocked; 
    public bool IsHighlighted => isHighlighted;

    public Slot(T _value)
    {
        value = _value;
    }
    public Slot(T _value, int _quantity)
    {
        value = _value;
        quantity = _quantity;
    }
    public Slot(bool _isBlocked)
    {
        isBlocked = _isBlocked;
    }
    public void SetNeighbor(Directions _direction , Slot<T> slot)
    {
        //neighbors[_direction] = slot;
        neighbors.Add(_direction, slot);
    }
    public bool GetNeighbor(Directions _direction , out Slot<T> slot)
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
    public bool CompareItem(T _value)
    {
        return value.Equals(_value);
    }
    public void SetValue(T _value)
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
    public Slot<T> Clone()
    {
        Slot<T> clone = new Slot<T>(value, quantity);
        
        return clone;
    }

    public Dictionary<Directions, Slot<T>> Neighbors => neighbors;
}

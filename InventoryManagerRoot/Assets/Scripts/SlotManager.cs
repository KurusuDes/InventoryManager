
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SlotManager<T>
{
    private Dictionary<int, Slot<T>> currentSlots = new();


    public  Action<int , Slot<T>> OnSlotAdded;
    public  Action<int ,Slot<T>> OnSlotUpdated;
    
    public  Action<int, int> OnSlotSwapped;
    
    public  Action<int> OnSlotCleared;

    public SlotManager(T value , int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            Slot<T> _slot = new Slot<T>(value, 0);
            currentSlots.Add(i, _slot);
        }
    }
    public void Add(int position, T value,int quantity = 0)
    {
        if (!currentSlots.TryGetValue(position, out Slot<T> slot))
        {
            Debug.LogError("Position not found in inventory");
            return;
        }
        if (slot.Value == null)
        {
            slot.SetValue(value);
            slot.AddQuantity(quantity);
            currentSlots[position] = slot;

            OnSlotAdded?.Invoke(position, slot);
            return;
        }
        if (slot.CompareItem(value))
        {
            slot.AddQuantity(quantity);
            currentSlots[position] = slot;

            OnSlotUpdated?.Invoke(position, slot);
        }
        else
        {
            Debug.LogWarning("Estas intentando añadir un item a un slot con un item disinto!");
        }
    }
    public void RemoveAt(int id)
    { 
        currentSlots[id].Clear();
    }
    public Slot<T> GetByKey(int key)
    { 
        return currentSlots[key];
    }
    public void Swap(int fromIndex , int toIndex)//-> ya no suma
    {
        
        if (!currentSlots.ContainsKey(fromIndex) || !currentSlots.ContainsKey(toIndex))
        {
            Debug.LogError("Índices inválidos.");
            return;
        }
        if (fromIndex == toIndex)
            return;

        Slot<T> fromSlot = currentSlots[fromIndex].Clone();
        Slot<T> toSlot = currentSlots[toIndex].Clone();

        if (fromSlot == null || fromSlot.Value == null)
        {
            Debug.LogWarning("No hay item para mover en el slot de origen.");
            return;
        }

        if (toSlot.Value == null || fromSlot.CompareItem(toSlot.Value))
        {
            Add(toIndex, fromSlot.Value, fromSlot.Quantity);
            currentSlots[fromIndex].Clear(); 
            Debug.Log($"Item movido de {fromIndex} a {toIndex}");

            OnSlotCleared?.Invoke(fromIndex);
        }
        else
        {
            currentSlots[fromIndex].Clear();
            currentSlots[toIndex].Clear();

            Debug.Log(fromSlot.Value +""+ toSlot.Value + "");
            Add(toIndex, fromSlot.Value, fromSlot.Quantity);
            Add(fromIndex, toSlot.Value, toSlot.Quantity);

            Debug.Log($"Items intercambiados entre {fromIndex} y {toIndex}");
        }
        OnSlotSwapped?.Invoke(fromIndex, toIndex);
    }

    public void SetNeighbors(int rows)
    {
        ClearDirections();

        int total = currentSlots.Count;
        if (total % rows != 0)
            throw new System.Exception("SlotManager: La cantidad de slots no forma una grilla con filas completas.");

        int cols = total / rows;

        for (int i = 0; i < total; i++)
        {
            int x = i % cols;
            int y = i / cols;

            foreach (Directions dir in Enum.GetValues(typeof(Directions)))
            {
                if (dir == Directions.None) continue;

                Vector2 offset = PosHelper(dir);
                int dx = (int)offset.x;
                int dy = (int)offset.y;

                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < cols && ny >= 0 && ny < rows)
                {
                    int neighborIndex = ny * cols + nx;

                    // Evitar auto-referencia o índices inválidos
                    if (neighborIndex != i && neighborIndex >= 0 && neighborIndex < currentSlots.Count)
                    {
                        var neighbor = currentSlots[neighborIndex];
                        if (neighbor != null)
                        {
                            currentSlots[i].SetNeighbor(dir, neighbor);
                        }
                    }
                }
            }
        }
    }
    public void Clear()
    {
        for (int i = 0; i < currentSlots.Count; i++)
        {
            currentSlots[i].Clear();
        }
    }
    public void ClearDirections()
    {
        for (int i = 0; i < currentSlots.Count; i++)
        {
            currentSlots[i].ClearDirections();
        }
    }
    public Vector2 PosHelper(Directions dir)
    {
        return dir switch
        {
            Directions.North => new Vector2Int(0, -1),      // arriba
            Directions.South => new Vector2Int(0, 1),       // abajo
            Directions.East => new Vector2Int(1, 0),        // derecha
            Directions.West => new Vector2Int(-1, 0),       // izquierda
            Directions.NorthEast => new Vector2Int(1, -1),  // arriba derecha
            Directions.NorthWest => new Vector2Int(-1, -1), // arriba izquierda
            Directions.SouthEast => new Vector2Int(1, 1),   // abajo derecha
            Directions.SouthWest => new Vector2Int(-1, 1),  // abajo izquierda
            _ => Vector2Int.zero
        };
    }

    public Dictionary<int, Slot<T>> CurrentSlots => currentSlots;


}

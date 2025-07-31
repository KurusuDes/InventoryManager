
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SlotManager<T>
{
    private Dictionary<int, Slot> currentSlots = new();


    public  Action<int , Slot> OnSlotAdded;
    public  Action<int ,Slot> OnSlotUpdated;
    
    public  Action<int, int> OnSlotSwapped;
    public Action OnChange;

    public  Action<int> OnSlotCleared;

    public SlotManager(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            Slot _slot = new Slot(0);
            currentSlots.Add(i, _slot);
        }
    }
    public void Add(int position, ItemSO itemSO,int quantity = 0)
    {
        if (!currentSlots.TryGetValue(position, out Slot slot))
        {
            Debug.LogError("Position not found in inventory");
            return;
        }
        if(slot.Item == null)
        {
            throw new ArgumentNullException(nameof(slot.Item), "El slot no tiene un item asignado.");
        }
        if (!slot.Item.HasItem())
        {
            slot.SetValue(itemSO);
            slot.AddQuantity(quantity);
            currentSlots[position] = slot;

            OnSlotAdded?.Invoke(position, slot);
            OnChange?.Invoke();
            //Debug.Log("ON CHANGE1");
            return;
        }
        if (slot.CompareSlot(itemSO))
        {
            slot.AddQuantity(quantity);
            currentSlots[position] = slot;

            OnSlotUpdated?.Invoke(position, slot);
            OnChange?.Invoke();
            Debug.Log("ON CHANGE2");
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
    public Slot GetByKey(int key)
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

        Slot fromSlot = currentSlots[fromIndex];
        Slot toSlot = currentSlots[toIndex];

        int fromQuantity = fromSlot.Quantity;
        int toQuantity = toSlot.Quantity;

        ItemSO fromItemSO = fromSlot.ItemSO;
        ItemSO toItemSO = toSlot.ItemSO;


        bool fromHasItem = fromSlot.Item.HasItem();
        bool toHasItem = toSlot.Item.HasItem();

        if (!fromHasItem && !toHasItem)
        {
            Debug.LogWarning("No hay item para mover en el slot de origen.");
            return;
        }
       
        if (!toHasItem || fromSlot.CompareSlot(toSlot.ItemSO))
        {
            Add(toIndex, fromItemSO, fromQuantity);
            currentSlots[fromIndex].Clear(); 
            Debug.Log($"Item movido de {fromIndex} a {toIndex}");

            OnSlotCleared?.Invoke(fromIndex);
        }
        else
        {
            currentSlots[fromIndex].Clear();
            currentSlots[toIndex].Clear();

            //Debug.Log(fromSlot.Item +""+ toSlot.Item + "");
            Add(toIndex, fromItemSO, fromQuantity);
            Add(fromIndex, toItemSO, toQuantity);

            Debug.Log($"Items intercambiados entre {fromIndex} y {toIndex}");
        }
        OnChange?.Invoke();
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

    public Dictionary<int, Slot> CurrentSlots => currentSlots;


}

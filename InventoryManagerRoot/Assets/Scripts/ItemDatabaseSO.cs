using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/Item Database")]
public class ItemDatabaseSO : ScriptableObject
{
    [Title("Serialized Items List (Editable)")]
    [SerializeField]
    private List<ItemSO> itemsList = new List<ItemSO>();


    //[ShowInInspector, ReadOnly]
    private HashSet<ItemSO> itemsDatabase = new HashSet<ItemSO>(new ItemEqualityComparer());


    public List<ItemSO> ItemsList => itemsList;
    public HashSet<ItemSO> Items => itemsDatabase;

    #region Unity Events

    private void OnEnable()
    {
        SynchronizeHashSet();
    }

    #endregion

    #region Public Methods

    [Button("Add Item (Safe Add)", ButtonSizes.Medium)]
    public void AddItem(ItemSO newItem)
    {
        if (newItem == null)
        {
            Debug.LogError("Cannot add null item to database.");
            return;
        }

        if (string.IsNullOrEmpty(newItem.ID.ToString()))
        {
            Debug.LogError($"Item '{newItem.name}' has no ID assigned.");
            return;
        }

        if (itemsDatabase.Any(i => i.ID == newItem.ID))
        {
            Debug.LogError($"Item with ID '{newItem.ID}' already exists in database.");
            return;
        }

        itemsList.Add(newItem);
        itemsDatabase.Add(newItem);

        Debug.Log($"Added item '{newItem.name}' with ID '{newItem.ID}'.");
    }

    [Button("Validate Database", ButtonSizes.Medium)]
    public void ValidateDatabase()
    {
        bool hasErrors = false;
        var idSet = new HashSet<string>();

        foreach (var item in itemsList)
        {
            if (item == null)
            {
                Debug.LogError("Found null item in database.");
                hasErrors = true;
                continue;
            }

            if (string.IsNullOrEmpty(item.ID.ToString()))
            {
                Debug.LogError($"Item '{item.name}' has no ID assigned.");
                hasErrors = true;
                continue;
            }

            if (!idSet.Add(item.ID.ToString()))
            {
                Debug.LogError($"Duplicate ID found: '{item.ID}'.");
                hasErrors = true;
            }
        }

        if (!hasErrors)
        {
            Debug.Log("Database validation successful - No errors found.");
        }
    }

    [Button("Read Database", ButtonSizes.Gigantic)]
    public void ReadAllDatabase()
    {
        foreach (var item in itemsDatabase)
        {
            Debug.Log($"Item ID: {item.ID}, Name: {item.EntityName}, Description: {item.Description}");
        }
    }

    public ItemSO GetItemByID(int id)
    {
        if (id == default)
        {
            Debug.LogError("Cannot search for null or empty ID.");
            return null;
        }

        var item = itemsDatabase.FirstOrDefault(i => i.ID == id);
        if (item == null)
        {
            Debug.LogError($"Item with ID '{id}' not found in database.");
        }
        return item;
    }
    public int GetCount()
    {
        return ItemsList.Count;
    }

    [Button("Synchronize List and HashSet", ButtonSizes.Medium)]
    public void SynchronizeHashSet()
    {
        itemsDatabase = new HashSet<ItemSO>(itemsList, new ItemEqualityComparer());
       // Debug.Log("Synchronized List with HashSet.");
    }

    #endregion

    #region Helper Classes

    private class ItemEqualityComparer : IEqualityComparer<ItemSO>
    {
        public bool Equals(ItemSO x, ItemSO y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            return x.ID == y.ID;
        }

        public int GetHashCode(ItemSO obj)
        {
            return obj.ID != 0 ? obj.ID : 0;
        }
    }

    #endregion
}
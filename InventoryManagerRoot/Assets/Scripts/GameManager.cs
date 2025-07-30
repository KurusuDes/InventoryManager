using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region References
    public ItemDatabaseSO itemDatabase;
    public Inventory inventory; 
    #endregion


    #region Singleton
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        // Implementación del patrón Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ItemSO GetItemByID(int itemID)
    {
        if (itemDatabase == null)
        {
            Debug.LogError("ItemDatabaseSO is not assigned in GameManager.");
            return null;
        }
        return itemDatabase.GetItemByID(itemID);
    }
    public void ReadItemById(int id)
    {
        ItemSO item = GetItemByID(id);
        if (item != null)
        {
            Debug.Log($"Item ID: {item.ID}, Name: {item.EntityName}, Description: {item.Description}");
        }
        else
        {
            Debug.LogError($"Item with ID {id} not found.");
        }
    }
}

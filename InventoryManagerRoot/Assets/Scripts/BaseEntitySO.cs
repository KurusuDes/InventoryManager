using UnityEngine;

[CreateAssetMenu(fileName = "BaseEntity", menuName = "ScriptableObjects/BaseEntity", order = 1)]
public class BaseEntitySO : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private string entityName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;

    
    public void Set(int id, string entityName, string description , Sprite icon)
    {
        this.id = id;
        this.entityName = entityName;
        this.description = description;
        this.icon = icon;   
    }


    public int ID => id;
    public string EntityName => entityName;
    public string Description => description;

    public Sprite Icon => icon;
}

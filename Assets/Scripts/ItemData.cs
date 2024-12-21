using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public float Weight;
    public string ItemName;
    public string Identifier;
    public ItemType Type;
    public Sprite Icon;

    private void Awake()
    {
        // Generate a unique random identifier for each item
        Identifier = System.Guid.NewGuid().ToString();
    }
}
// Enum for different item types
public enum ItemType
{
    RedItem,
    GreenItem,
    BlueItem
}

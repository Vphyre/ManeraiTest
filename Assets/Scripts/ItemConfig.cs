using UnityEngine;

public class ItemConfig : MonoBehaviour
{
    [SerializeField] private ItemData itemData; // Reference to the ScriptableObject ItemData

    // Private variables to hold item data
    private float weight;    // Weight of the item
    private string itemName; // Name of the item
    private string identifier; // Unique identifier for the item
    private ItemType type;   // Type of the item
    private Sprite _icon; // Icon of the item

    // Public properties to access private fields 
    public float Weight { get { return weight; } }
    public string ItemName { get { return itemName; } }
    public string Identifier { get { return identifier; } }
    public ItemType Type { get { return type; } }
    public Sprite Icon { get { return _icon; } }

    private void Awake()
    {
        // Initialize the item data from the ScriptableObject ItemData
        weight = itemData.Weight;
        itemName = itemData.ItemName;
        identifier = itemData.Identifier;
        type = itemData.Type;
        _icon = itemData.Icon;
    }
}

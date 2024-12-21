using UnityEngine;

public class ItemDragHandler : MonoBehaviour
{
    [SerializeField] private Backpack _backpack;

    // Private variables for handling item drag
    private Vector3 offset;
    private ItemConfig itemConfig;
    private bool isOnBackpack = false;
    private bool isDragging = false;
    private Camera mainCamera;
    private Rigidbody rb;
    private Collider objCollider;

    private void Start()
    {
        // Initialize references to main camera, Rigidbody, Collider, and ItemConfig
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        objCollider = GetComponent<Collider>();
        itemConfig = GetComponent<ItemConfig>();
    }

    private void OnMouseDown()
    {
        if (isOnBackpack)
        {
            return; // Exit if the item is already on the backpack
        }
        isDragging = true;
        offset = transform.position - GetMouseWorldPos(); // Calculate the offset
        rb.useGravity = false; // Disable gravity while dragging
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + offset; // Update position based on mouse
            rb.velocity = Vector3.zero; // Stop any movement
            rb.angularVelocity = Vector3.zero; // Stop any rotation
        }
    }

    private void OnMouseUp()
    {
        if (isOnBackpack)
        {
            return; // Exit if the item is already on the backpack
        }
        OnDropItem();
        rb.useGravity = true; // Re-enable gravity when dropping
    }

    public void OnDropItem()
    {
        isDragging = false;
        rb.velocity = Vector3.zero; // Stop any movement
        rb.angularVelocity = Vector3.zero; // Stop any rotation
    }

    public void OnDropItemOnBackpack()
    {
        if (isOnBackpack)
        {
            return; // Exit if the item is already on the backpack
        }
        if (_backpack != null)
        {
            OnDropItem();
            objCollider.enabled = false;
            Invoke("EnableInteractionByTime", 1f); // Disable interaction temporarily
            _backpack.AddItem(itemConfig, gameObject, this); // Add item to the backpack
            transform.SetParent(_backpack.transform);
            rb.constraints = RigidbodyConstraints.FreezeAll; // Freeze item movement
        }
    }

    public void OnRemoveItemBackpack()
    {
        transform.SetParent(null);
        _backpack.RemoveItem(itemConfig, gameObject, this); // Remove item from the backpack
        OnDropItem();
        rb.constraints = RigidbodyConstraints.None; // Unfreeze item movement
        isOnBackpack = false;
    }

    private void EnableInteractionByTime()
    {
        objCollider.enabled = true; // Re-enable interaction
        isOnBackpack = true;
    }

    private Vector3 GetMouseWorldPos()
    {
        // Get the mouse position in world space
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }
}

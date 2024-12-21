using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class BackpackVisualController : MonoBehaviour
{
    [SerializeField] private List<Image> itemIcons; // Icons for items in the inventory
    [SerializeField] private Material highlightMaterial; // Highlight material for visual effect
    [SerializeField] private Material defaultMaterial; // Default material
    [SerializeField] private UnityEvent OnShowInventory; // Event triggered on showing inventory
    [SerializeField] private UnityEvent OnHideInventory; // Event triggered on hiding inventory

    private Camera mainCamera;
    private Renderer backpackRenderer;
    private bool isMouseOver;
    private bool holdingLeftMouseButton;

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
        backpackRenderer = GetComponent<Renderer>(); // Get the Renderer component
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform)
            {
                OnPointerEnter(); // Mouse is over the backpack
            }
            else
            {
                OnPointerExit(); // Mouse is not over the backpack
            }
        }
        else
        {
            OnPointerExit(); // Mouse is not over any object
        }

        if (isMouseOver && Input.GetMouseButton(0)) // Check if the mouse is over and the LMB is pressed
        {
            holdingLeftMouseButton = true;
            backpackRenderer.material = highlightMaterial; // Apply highlight material
            ShowInventory(); // Show the inventory UI
        }

        if (holdingLeftMouseButton && !Input.GetMouseButton(0)) // Check if the LMB was released
        {
            holdingLeftMouseButton = false;
            backpackRenderer.material = defaultMaterial; // Revert to default material
            isMouseOver = false;
            HideInventory(); // Hide the inventory UI
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Item")
                {
                    hit.transform.gameObject.GetComponent<ItemDragHandler>().OnRemoveItemBackpack(); // Remove item from backpack
                }
            }
        }
    }

    private void OnPointerEnter()
    {
        if (!isMouseOver)
        {
            isMouseOver = true; // Set flag to indicate mouse is over the backpack
        }
    }

    private void OnPointerExit()
    {
        if (isMouseOver)
        {
            // Function that would be executed if necessary
        }
    }

    private void ShowInventory()
    {
        OnShowInventory.Invoke(); // Invoke the event to show inventory
    }

    private void HideInventory()
    {
        OnHideInventory.Invoke(); // Invoke the event to hide inventory
    }

    public void SetImageIcon(int index, Sprite itemIcon)
    {
        itemIcons[index].sprite = itemIcon; // Set the icon for the specified item
        itemIcons[index].enabled = true; // Enable the icon
    }

    public void RemoveImageIcon(int index)
    {
        itemIcons[index].enabled = false; // Disable the icon for the specified item
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;
using Utilities;

public class Backpack : MonoBehaviour
{
    [SerializeField] private List<Transform> itemsPlacesPositions; // List of items positions in the backpack
    [SerializeField] private List<Transform> dropPlacesPositions; // List of drop places positions

    // UnityEvents for additional actions
    [SerializeField] private UnityEvent onItemAdded;
    [SerializeField] private UnityEvent onItemRemoved;
    [SerializeField] private UnityEvent onSendItemEvent;

    private List<ItemConfig> items; // List of items in the backpack
    private BackpackVisualController backpackUI;

    private void Awake()
    {
        // Initialize the list of items
        items = new List<ItemConfig>();
        backpackUI = GetComponent<BackpackVisualController>();
    }

    // Method to add an item to the backpack
    public void AddItem(ItemConfig item, GameObject itemObj, ItemDragHandler itemDragHandler)
    {
        if (!items.Contains(item))
        {
            items.Add(item);

            // Move the item to its designated position in the backpack
            ObjectMovementUtility.MoveToPosition(itemsPlacesPositions[SetPositionByType(item)], itemObj, 1f);
            backpackUI.SetImageIcon(SetPositionByType(item), item.Icon);

            // Invoke the event for adding an item
            onItemAdded.Invoke();

            // Send the item addition event to the server
            SendItemEvent(item.Identifier, "added");
        }
    }

    // Method to remove an item from the backpack
    public void RemoveItem(ItemConfig item, GameObject itemObj, ItemDragHandler itemDragHandler)
    {
        if (items.Contains(item))
        {
            items.Remove(item);

            // Remove the item's icon and move it to the drop position
            backpackUI.RemoveImageIcon(SetPositionByType(item));
            ObjectMovementUtility.MoveToPosition(dropPlacesPositions[SetPositionByType(item)], itemObj, 1f);

            // Invoke the event for removing an item
            onItemRemoved.Invoke();

            // Send the item removal event to the server
            SendItemEvent(item.Identifier, "removed");
        }
    }

    // Determine the position for the item based on its type
    private int SetPositionByType(ItemConfig item)
    {
        if (item.Type == ItemType.RedItem)
        {
            return 0;
        }
        else if (item.Type == ItemType.GreenItem)
        {
            return 1;
        }
        else if (item.Type == ItemType.BlueItem)
        {
            return 2;
        }
        return 0; // Default position if the type does not match
    }
    
    // Method to send item events to the server
    private void SendItemEvent(string itemId, string eventType)
    {
        StartCoroutine(SendPostRequest(itemId, eventType));
    }

    // Coroutine to handle sending POST requests
    private IEnumerator SendPostRequest(string itemId, string eventType)
    {
        string serverUrl = "https://wadahub.manerai.com/api/inventory/status";
        string authToken = "Bearer kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP";

        // Create a JSON object with item details and event type
        string json = JsonUtility.ToJson(new
        {
            itemId = itemId,
            eventType = eventType
        });

        // Create the request and set its headers
        UnityWebRequest request = new UnityWebRequest(serverUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", authToken);

        // Invoke the event for sending item event
        onSendItemEvent.Invoke();

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Request sent successfully");
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Request failed");
            Debug.Log(request.error);
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField] private string targetTag; // Tag of the target objects to detect collisions with

    // UnityEvents to trigger on collision events
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerStay;
    public UnityEvent onTriggerExit;

    // Called when another collider enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            onTriggerEnter.Invoke(); // Invoke the event for entering the trigger
        }
    }

    // Called when another collider stays within the trigger
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            onTriggerStay.Invoke(); // Invoke the event for staying in the trigger
        }
    }

    // Called when another collider exits the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            onTriggerExit.Invoke(); // Invoke the event for exiting the trigger
        }
    }
}

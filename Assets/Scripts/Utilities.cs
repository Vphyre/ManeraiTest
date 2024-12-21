using UnityEngine;
using System.Collections;

namespace Utilities
{
    public static class ObjectMovementUtility
    {
        private static Coroutine coroutine;

        // Nested class to manage coroutines
        public class CoroutineRunner : MonoBehaviour
        {
            private static CoroutineRunner _instance;

            // Singleton instance of CoroutineRunner
            public static CoroutineRunner Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("CoroutineRunner");
                        _instance = obj.AddComponent<CoroutineRunner>();
                        DontDestroyOnLoad(obj);
                    }
                    return _instance;
                }
            }
        }

        // Method to initiate the movement of an object to a target position over a specified duration
        public static void MoveToPosition(Transform targetPosition, GameObject objectToMove, float duration)
        {
            coroutine = CoroutineRunner.Instance.StartCoroutine(ObjectMovement(targetPosition, objectToMove, duration));
        }

        // Coroutine to smoothly move the object to the target position
        private static IEnumerator ObjectMovement(Transform targetPosition, GameObject objectToMove, float duration)
        {
            Vector3 startPosition = objectToMove.transform.position; // Initial position of the object
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                // Interpolate the position of the object between start and target positions over time
                objectToMove.transform.position = Vector3.Lerp(startPosition, targetPosition.position, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the object reaches the exact target position at the end of the duration
            objectToMove.transform.position = targetPosition.position;
            CoroutineRunner.Instance.StopCoroutine(coroutine); // Stop the coroutine after movement is complete
        }
    }
}

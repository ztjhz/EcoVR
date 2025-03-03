using UnityEngine;
using UnityEngine.EventSystems;

public class UIInteractionDebugger : MonoBehaviour
{
    void Update()
    {
        // Detect if pointer is over any UI element
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UIInteractionDebugger: Pointer is over a UI element.");
        }
        else
        {
            Debug.Log("UIInteractionDebugger: Pointer is NOT over UI.");
        }

        // Detect mouse button press
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("UIInteractionDebugger: Left mouse button clicked.");
        }
    }
}

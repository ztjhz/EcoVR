using UnityEngine;
using UnityEngine.EventSystems;

public class ShowCursor : MonoBehaviour
{
    void Update()
    {
        // Only show cursor if interacting with UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            Debug.LogError("=====================> true");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Debug.LogError("=====================> false");
            // If not over UI, you can lock the cursor (optional)
            Cursor.visible = false;  // You can hide the cursor when not interacting with UI.
            Cursor.lockState = CursorLockMode.Locked; // Optional: Locks the cursor in the center when not over UI.
        }
    }
}

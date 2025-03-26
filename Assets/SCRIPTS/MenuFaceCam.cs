using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuFaceCam : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera != null)
        {
            // Rotate only, do not adjust position
            Vector3 direction = mainCamera.transform.position - transform.position;
            direction.y = 0; // Keep it upright

            if (direction.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(-direction);
            }
        }
    }
}

using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class CameraZoomController : MonoBehaviour
{
    public XROrigin xrOrigin;
    public Slider zoomSlider;

    private float minHeight = 90f; // The closest zoom-in position
    private float maxHeight; // Will be set dynamically to current camera height

    void Start()
    {
        if (xrOrigin == null)
        {
            Debug.LogError("CameraZoomController: XROrigin is not assigned!");
            return;
        }

        if (zoomSlider == null)
        {
            //Debug.LogError("CameraZoomController: Slider is not assigned!");
            return;
        }

        // Set maxHeight to the xrOrigin's starting height (zoomed out)
        maxHeight = xrOrigin.transform.position.y;

        // Ensure minHeight is valid (so we don't get inverted range issues)
        if (minHeight > maxHeight)
        {
            minHeight = maxHeight - 50f; // Keep a reasonable zoom range
            Debug.LogWarning($"CameraZoomController: minHeight was too high. Adjusted to {minHeight}");
        }

        // Configure slider values
        zoomSlider.minValue = minHeight;
        zoomSlider.maxValue = maxHeight;
        zoomSlider.value = maxHeight; // Start at the current camera height (zoomed out)

        // Log to debug values
        Debug.Log($"CameraZoomController: Initial XROrigin height = {maxHeight}");
        Debug.Log($"CameraZoomController: Slider range set to Min: {zoomSlider.minValue}, Max: {zoomSlider.maxValue}");

        // Add listener for zoom control
        zoomSlider.onValueChanged.AddListener(UpdateCameraZoom);
    }

    void UpdateCameraZoom(float value)
    {
        Debug.Log($"CameraZoomController: Slider value changed to {value}");

        // Adjust camera height (move downward to zoom in)
        Vector3 newPos = xrOrigin.transform.position;
        newPos.y = value;
        xrOrigin.transform.position = newPos;

        Debug.Log($"XROrigin position updated to: {xrOrigin.transform.position}");
    }
}

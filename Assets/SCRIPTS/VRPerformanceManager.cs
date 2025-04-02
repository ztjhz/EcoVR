using UnityEngine;
using UnityEngine.XR;

public class VRPerformanceManager : MonoBehaviour
{
    [Header("Render Scale")]
    [Range(0.5f, 1.0f)]
    public float renderScale = 0.75f;

    [Header("Dynamic Resolution Settings")]
    [Range(0.5f, 1.0f)] public float minDynamicScale = 0.6f;
    [Range(0.5f, 1.0f)] public float maxDynamicScale = 1.0f;

    private float dynamicScale = 1.0f;

    void Awake()
    {
        // Set fixed render scale early
        XRSettings.eyeTextureResolutionScale = renderScale;
        Debug.Log("Render scale set to: " + renderScale);

        // Initialize dynamic resolution to max
        ScalableBufferManager.ResizeBuffers(maxDynamicScale, maxDynamicScale);
    }

    void Update()
    {
        // Simulate GPU load fluctuation (replace with real metrics later)
        float simulatedGpuLoad = Mathf.PingPong(Time.time * 0.1f, 1f); // fluctuates 0–1

        // Calculate dynamic scale
        dynamicScale = Mathf.Lerp(minDynamicScale, maxDynamicScale, 1f - simulatedGpuLoad);

        // Apply dynamic resolution
        ScalableBufferManager.ResizeBuffers(dynamicScale, dynamicScale);
    }
}

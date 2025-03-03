using UnityEngine;
using UnityEngine.UI;
using Tenkoku.Core; // Ensure this is included

public class TimeLapseController : MonoBehaviour
{
    public Slider timeSlider;
    public TenkokuModule tenkokuSky; // Reference to Tenkoku system

    // Define exact speeds that match Tenkoku's "Advance Time x" setting
    private float[] timeSpeeds = { 1f, 1000f, 10000f, 100000f, 1000000f }; // 5 distinct values

    void Start()
    {
        if (timeSlider == null)
        {
            Debug.LogError("TimeLapseController: Slider is not assigned!");
            return;
        }

        if (tenkokuSky == null)
        {
            Debug.LogError("TimeLapseController: Tenkoku Sky system is NOT assigned! Drag the TenkokuModule instance from the scene.");
            return;
        }

        Debug.Log("TimeLapseController: Tenkoku Sky system assigned successfully.");

        // Set slider values
        timeSlider.minValue = 0;
        timeSlider.maxValue = timeSpeeds.Length - 1;
        timeSlider.wholeNumbers = true;

        // Set default time lapse speed
        UpdateTimeLapse(timeSlider.value);

        // Add listener to slider
        timeSlider.onValueChanged.AddListener(UpdateTimeLapse);
    }

    void UpdateTimeLapse(float value)
    {
        if (tenkokuSky == null) return;

        int index = Mathf.Clamp((int)value, 0, timeSpeeds.Length - 1);
        float selectedSpeed = timeSpeeds[index];

        // Ensure Tenkoku is set to auto advance
        tenkokuSky.autoTime = true;  
        tenkokuSky.enableAutoAdvance = true;

        // Apply the selected speed to Tenkoku’s time compression setting
        tenkokuSky.useTimeCompression = selectedSpeed;

        // Directly update the Inspector value for "Advance Time x"
        tenkokuSky.timeCompression = selectedSpeed;

        // Manually force Tenkoku to refresh time calculations
        tenkokuSky.Invoke("TimeUpdate", 0);

        Debug.Log($"TimeLapseController: Time Speed set to x{selectedSpeed} (Slider Value: {value})");
    }
}

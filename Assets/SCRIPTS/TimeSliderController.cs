using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TimeSliderController : MonoBehaviour
{
    public Slider timeSlider; // Assign this in Unity Inspector
    private VisualisePopulationDistribution visualiser;
    private Dictionary<int, List<AnimalDataPoint>> animalHistoryFromTime;
    private List<int> availableTimeStamps; // Stores valid time points

    private void Start()
    {
        StartCoroutine(WaitForVisualisation());
    }

    private IEnumerator WaitForVisualisation()
    {
        // Wait for the VisualisePopulationDistribution script to initialize
        while (FindObjectOfType<VisualisePopulationDistribution>() == null)
        {
            yield return null; // Wait one frame
        }

        visualiser = FindObjectOfType<VisualisePopulationDistribution>();

        // Fetch animal history
        List<AnimalDataPoint> animalHistory = AnimalAnalytics.Instance?.GetAnimalHistory();
        if (animalHistory == null || animalHistory.Count == 0)
        {
            Debug.LogWarning("No animal data found! Slider will not be interactive.");
            timeSlider.interactable = false;
            yield break;
        }

        // Group data by time and extract valid timestamps
        animalHistoryFromTime = animalHistory
            .GroupBy(a => a.time)
            .ToDictionary(g => g.Key, g => g.ToList());

        availableTimeStamps = animalHistoryFromTime.Keys.OrderBy(t => t).ToList();

        if (availableTimeStamps.Count == 0)
        {
            Debug.LogWarning("No valid timestamps found.");
            timeSlider.interactable = false;
            yield break;
        }

        SetupSlider();
    }

    private void SetupSlider()
    {
        if (timeSlider == null)
        {
            Debug.LogError("Time Slider not assigned in Inspector!");
            return;
        }

        timeSlider.minValue = 0;
        timeSlider.maxValue = availableTimeStamps.Count - 1; // Use index-based values
        timeSlider.wholeNumbers = true; // Snaps to integer notches
        timeSlider.value = 0; // Default to the first recorded timestamp
        timeSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });

        // Initialize the graph with the first available timestamp
        UpdateGraphAtCurrentIndex();
    }

    private void OnSliderValueChanged()
    {
        UpdateGraphAtCurrentIndex();
    }

    private void UpdateGraphAtCurrentIndex()
    {
        int selectedIndex = (int)timeSlider.value;
        int selectedTime = availableTimeStamps[selectedIndex]; // Get actual time from index
        visualiser.UpdateGraph(selectedTime);
    }
}

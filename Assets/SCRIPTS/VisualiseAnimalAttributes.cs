using UnityEngine;
using System.Collections.Generic;
using XCharts.Runtime;
using System.Linq;
using UnityEngine.UI;

public class VisualiseAnimalAttributes : MonoBehaviour
{
    public GameObject chartPrefab;  // Prefab for the radar chart

    private RadarChart predatorChart;  // The predator radar chart
    private RadarChart preyChart;      // The prey radar chart

    private void Awake()
    {
        // Check if the chart prefab is assigned
        if (chartPrefab == null)
        {
            Debug.LogError("Chart prefab is missing! Assign a RadarChart prefab.");
            return;
        }

        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Radar") ||
                child.name.StartsWith("serie") ||
                child.name.StartsWith("painter_") ||
                child.name.StartsWith("Title") ||
                child.name.StartsWith("Tooltip"))
            {
                Destroy(child.gameObject);
            }
        }

        Debug.Log($"Chart Prefab Assigned: {chartPrefab != null}");

        // Instantiate separate charts for predator and prey stats
        predatorChart = InstantiateChart("Predator Stats", new Vector2(-350, 0));
        preyChart = InstantiateChart("Prey Stats", new Vector2(350, 0));

        // Populate both charts
        PopulateCharts();
    }

    private RadarChart InstantiateChart(string chartName, Vector2 position)
    {
        Debug.Log("Creating chart: " + chartName);

        GameObject newChart = Instantiate(chartPrefab, transform);
        newChart.GetComponent<RectTransform>().anchoredPosition = position;
        newChart.name = chartName;

        newChart.SetActive(true);
        Debug.Log("Chart Active: " + newChart.activeSelf);

        RadarChart chartComponent = newChart.GetComponent<RadarChart>();
        if (chartComponent == null)
        {
            Debug.LogError("Failed to instantiate RadarChart from prefab.");
            return null;
        }

        // Initialize and configure the chart
        chartComponent.Init();
        chartComponent.SetSize(700, 800);

        // Set title
        var title = chartComponent.EnsureChartComponent<Title>();
        title.text = chartName;

        return chartComponent;
    }

    private void PopulateCharts()
    {
        // Define hardcoded stats for predator and prey
        Dictionary<string, float> predatorStats = new Dictionary<string, float>
        {
            { "Detection Range", 30f },
            { "Walking Speed", 4.5f },
            { "Hunting Speed", 8.5f },
            { "Attack Range", 3.5f },
            { "Attack Cooldown", 2f }
        };

        Dictionary<string, float> preyStats = new Dictionary<string, float>
        {
            { "Move Speed", 2f },
            { "Rotation Speed", 100f },
            { "Detection Range", 10f },
            { "Flee Distance", 20f }
        };

        if (predatorChart != null)
        {
            Debug.Log("Predator Chart Found - Visualizing Stats...");
            VisualizeStats(predatorChart, "Predator", predatorStats);
        }
        else Debug.LogError("Predator Chart is NULL!");

        if (preyChart != null)
        {
            Debug.Log("Prey Chart Found - Visualizing Stats...");
            VisualizeStats(preyChart, "Prey", preyStats);
        }
        else Debug.LogError("Prey Chart is NULL!");
    }

    private void VisualizeStats(RadarChart chart, string animalName, Dictionary<string, float> stats)
    {
        if (chart == null || stats == null || stats.Count == 0)
        {
            Debug.LogWarning($"Chart or stats are missing for {animalName}!");
            return;
        }

        Debug.Log($"Visualizing stats for {animalName} with {stats.Count} stats.");

        // Reset indicators
        ResetRadarIndicators(chart, stats.Keys.ToList());

        // Ensure Radar Serie exists
        Serie serie = chart.GetSerie<Radar>(0);
        if (serie == null)
        {
            Debug.LogWarning($"No Radar series found in {animalName}, adding a new one.");
            serie = chart.AddSerie<Radar>("Animal Stats");
            serie.radarIndex = 0;
        }

        if (serie != null && serie.dataCount > 0)
        {
            // Clear existing data and add the new data
            serie.ClearData();  // Clear old data
            serie.AddData(stats.Values.Select(v => (double)v).ToList()); // Add new data
        }
        else
        {
            // Add new data if no data exists
            chart.ClearData();
            chart.AddData(0, stats.Values.Select(v => (double)v).ToList());
        }

        chart.RefreshChart();
        Debug.Log("Chart Updated Successfully.");
    }

    private void ResetRadarIndicators(RadarChart chart, List<string> statNames)
    {
        var radarCoord = chart.GetChartComponent<RadarCoord>() ?? chart.AddChartComponent<RadarCoord>();
        radarCoord.shape = RadarCoord.Shape.Polygon;
        radarCoord.center[0] = 0.5f;
        radarCoord.center[1] = 0.5f;
        radarCoord.radius = 0.25f;

        // Clear previous indicators and add new ones
        radarCoord.indicatorList.Clear();
        Debug.Log($"Resetting Radar Indicators for {statNames.Count} stats.");

        foreach (var stat in statNames)
        {
            radarCoord.AddIndicator(stat, 0, 100);
            Debug.Log($"Added Indicator: {stat}");
        }

        // Ensure the chart refreshes after adding indicators
        chart.RefreshChart();
    }
}

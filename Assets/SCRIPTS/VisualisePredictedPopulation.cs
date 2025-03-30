using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using XCharts.Runtime;
using System.Linq;

public class VisualisePredictedPopulation : MonoBehaviour
{
    public int predictionTime = 30; // Predict 30s ahead
    public int timeStep = 10; // Fixed time interval for prediction
    private float startTime;

    private List<int> timePoints = new List<int>();
    private Dictionary<string, List<int>> populationData = new Dictionary<string, List<int>>();
    private LineChart chart;

    void Awake()
    {
        chart = gameObject.GetComponent<LineChart>();
        if (chart == null)
        {
            Debug.LogWarning("Chart missing! Adding a chart now...");
            CreateGraph();
        }

        var title = chart.EnsureChartComponent<Title>();
        title.text = "Predicted Animal Population Over Time";

        startTime = Time.time;
        GeneratePopulationForecast();
        UpdateLineChart();
    }

    void Update()
    {
        if (Time.time - startTime >= (timePoints.Count * timeStep))
        {
            GeneratePopulationForecast();
            UpdateLineChart();
        }
    }

    void GeneratePopulationForecast()
    {
        int lastTime = timePoints.Count > 0 ? timePoints.Last() : 0;
        int futureTime = lastTime + timeStep;
        timePoints.Add(futureTime);

        var animalCounts = AnimalAnalytics.Instance.GetAnimalCount();
        Debug.Log("Animal Counts Retrieved: " + string.Join(", ", animalCounts.Select(a => a.Key + ": " + a.Value)));

        var groupedCounts = animalCounts
            .GroupBy(entry => AnimalAnalytics.CleanAnimalName(entry.Key))
            .ToDictionary(group => group.Key, group => group.Sum(entry => entry.Value));

        foreach (var animalName in groupedCounts.Keys)
        {
            string animalType = AnimalAnalytics.Instance.GetAnimalType(animalName);
            Debug.Log($"Predicting for {animalName} of type {animalType}");

            int predictedValue = PredictFuturePopulation(animalName, animalType, futureTime);
            Debug.Log($"Predicted value for {animalName}: {predictedValue}");

            if (!populationData.ContainsKey(animalName))
                populationData[animalName] = new List<int>();

            populationData[animalName].Add(predictedValue);
        }
    }

    float ARIMAPrediction(string species, float time)
    {
        List<int> pastData = AnimalAnalytics.Instance.GetAnimalHistory()
            .Where(data => AnimalAnalytics.CleanAnimalName(data.animalName) == species)
            .OrderBy(data => data.time)
            .Select(data => data.count)
            .ToList();

        if (pastData.Count < 3)
            return pastData.LastOrDefault(); // Return last value or 0

        float trend = pastData.Last() - pastData[pastData.Count - 2];
        return Mathf.Max(pastData.Last() + trend * time, 0);
    }

    int PredictFuturePopulation(string species, string animalType, float time)
    {
        return Mathf.RoundToInt(ARIMAPrediction(species, time));
    }

    void UpdateLineChart()
    {
        chart.RemoveData();

        // Update X Axis
        foreach (int time in timePoints)
            chart.AddXAxisData(time.ToString());

        // Update series for each animal group
        foreach (var entry in populationData)
        {
            Line serie = chart.AddSerie<Line>(entry.Key);
            serie.symbol.type = SymbolType.EmptyCircle;
            serie.lineStyle.type = LineStyle.Type.Dashed;

            foreach (int value in entry.Value)
                chart.AddData(entry.Key, value);
        }
    }

    void CreateGraph()
    {
        chart = gameObject.AddComponent<LineChart>();
        chart.Init();
        chart.SetSize(1200, 700);

        var title = chart.EnsureChartComponent<Title>();
        title.text = "Predicted Animal Population Over Time";
        title.show = true;

        var legend = chart.EnsureChartComponent<Legend>();
        legend.show = true;

        var xAxis = chart.EnsureChartComponent<XAxis>();
        xAxis.show = true;
        xAxis.splitNumber = 5;

        var yAxis = chart.EnsureChartComponent<YAxis>();
        yAxis.show = true;

        chart.RefreshChart();
    }
}

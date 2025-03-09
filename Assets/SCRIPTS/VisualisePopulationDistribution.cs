using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using XCharts.Runtime;

public class VisualisePopulationDistribution : MonoBehaviour
{
    private ScatterChart chart;
    private Dictionary<int, List<AnimalDataPoint>> animalHistoryFromTime;
    private Dictionary<string, int> graphIndexFromAnimalCategory;
    private bool isInitialized = false;

    private void Awake()
    {
        // Ensure ScatterChart is assigned
        chart = gameObject.GetComponent<ScatterChart>();
        if (chart == null)
        {
            Debug.LogWarning("Chart missing! Creating one now...");
            CreateGraph();
        }

        // Fetch animal history data
        List<AnimalDataPoint> animalHistory = AnimalAnalytics.Instance?.GetAnimalHistory();

        if (animalHistory == null || animalHistory.Count == 0)
        {
            Debug.LogWarning("No animal data found! Graph will not update.");
            return;
        }

        // Group animal data by time
        animalHistoryFromTime = animalHistory
            .GroupBy(a => a.time)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Ensure there is at least one time entry
        if (animalHistoryFromTime.Count == 0)
        {
            Debug.LogWarning("No valid time data found.");
            return;
        }

        isInitialized = true;

        // Display the first available time step
        int firstTime = animalHistoryFromTime.Keys.First();
        UpdateGraph(firstTime);
    }

    public void UpdateGraph(int time)
    {
        // Prevent updating before initialization
        if (!isInitialized)
        {
            Debug.LogWarning("Graph not initialized yet.");
            return;
        }

        // Ensure the requested time exists
        if (animalHistoryFromTime == null || !animalHistoryFromTime.ContainsKey(time))
        {
            Debug.LogWarning($"No data available for time: {time}");
            return;
        }

        // Clear previous graph data
        chart.RemoveData();
        graphIndexFromAnimalCategory = new Dictionary<string, int>();

        int graphCount = 0;

        foreach (AnimalDataPoint data in animalHistoryFromTime[time])
        {
            string animalCategory = data.animalType;

            if (!graphIndexFromAnimalCategory.ContainsKey(animalCategory))
            {
                graphIndexFromAnimalCategory[animalCategory] = graphCount;
                Scatter serie = chart.AddSerie<Scatter>(animalCategory);
                serie.symbol.type = SymbolType.Circle;
                serie.symbol.size = 10;
                serie.itemStyle.opacity = 0.5f;
                graphCount++;
            }

            int index = graphIndexFromAnimalCategory[animalCategory];

            foreach (Vector3 pos in data.positions)
            {
                chart.AddData(index, new double[] { pos.x, pos.z }); // Using X-Z plane
            }
        }
    }

    private void CreateGraph()
    {
        chart = gameObject.AddComponent<ScatterChart>();
        chart.Init();
        chart.SetSize(1300, 700);

        var title = chart.EnsureChartComponent<Title>();
        title.text = "Population Distribution";
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using XCharts.Runtime;
using static XCharts.Runtime.Axis;

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

        isInitialized = true;

        FixChartAxes();

        // Display the first available time step
        int firstTime = animalHistoryFromTime.Keys.First();
        UpdateGraph(firstTime);
    }

    private void FixChartAxes()
    {
        double minX, maxX, minZ, maxZ;

        minX = double.MaxValue;
        maxX = double.MinValue;
        minZ = double.MaxValue;
        maxZ = double.MinValue;

        foreach (var timeStep in animalHistoryFromTime.Values)
        {
            foreach (var data in timeStep)
            {
                foreach (Vector3 pos in data.positions)
                {
                    if (pos.x < minX) minX = pos.x;
                    if (pos.x > maxX) maxX = pos.x;
                    if (pos.z < minZ) minZ = pos.z;
                    if (pos.z > maxZ) maxZ = pos.z;
                }
            }
        }

        // Ensure reasonable defaults if no data exists
        if (minX == double.MaxValue) minX = 0;
        if (maxX == double.MinValue) maxX = 300;
        if (minZ == double.MaxValue) minZ = 0;
        if (maxZ == double.MinValue) maxZ = 300;

        chart.GetChartComponent<XAxis>().minMaxType = AxisMinMaxType.Custom;
        chart.GetChartComponent<XAxis>().min = minX - 10;
        chart.GetChartComponent<XAxis>().max = maxX + 10;

        chart.GetChartComponent<YAxis>().minMaxType = AxisMinMaxType.Custom;
        chart.GetChartComponent<YAxis>().min = minZ - 10;
        chart.GetChartComponent<YAxis>().max = maxZ + 10;
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
            // Group all the same species together (e.g. Deer_v4 and Deer_v5)
            string animalCategory = AnimalAnalytics.CleanAnimalName(data.animalName);

            if (!graphIndexFromAnimalCategory.ContainsKey(animalCategory))
            {
                graphIndexFromAnimalCategory[animalCategory] = graphCount;
                Scatter serie = chart.AddSerie<Scatter>(animalCategory);

                if (data.animalType == "prey")
                    serie.symbol.type = SymbolType.Circle;
                else
                    serie.symbol.type = SymbolType.Triangle;

                serie.symbol.size = 10;
                serie.itemStyle.opacity = 0.8f;
                serie.AnimationEnable(false);
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

        // TODO: continue customising (not the focus now, as the graph should already exist)

        // set x-axis and y-axis name
        // remove x-axis and y-axis ticks and labels
        // remove y-axis split lines
        // add legend
    }
}

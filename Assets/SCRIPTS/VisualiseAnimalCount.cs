using UnityEngine;
using System.Collections.Generic;
using XCharts.Runtime;
using System.Linq;

public class VisualiseAnimalCount : MonoBehaviour
{
    private void Awake()
    {
        PieChart chart = gameObject.GetComponent<PieChart>();
        if (chart == null)
        {
            Debug.LogWarning("Chart missing! Adding a chart now...");
            CreateGraph(chart);
        }

        Dictionary<string, int> animalCount = AnimalAnalytics.Instance.GetAnimalCount()
            .GroupBy(entry => AnimalAnalytics.CleanAnimalName(entry.Key))
            .ToDictionary(group => group.Key, group => group.Sum(entry => entry.Value));

        Pie serie = chart.GetSerie<Pie>(0);
        serie.ClearData();

        foreach (var entry in animalCount)
        {
            string animalName = entry.Key;
            int count = entry.Value;
            chart.AddData(serie.index, count, animalName);
        }
    }

    private void CreateGraph(PieChart chart)
    {
        // Fall back
        chart = gameObject.AddComponent<PieChart>();
        chart.Init();
        chart.SetSize(1200, 700);

        var title = chart.EnsureChartComponent<Title>();
        title.text = "Population Count";

        // todo: continue customising (not the focus now, as the graph should already exist

        // add legend
        // set x-axis and y-axis title
        // remove x-axis ticks
    }
}

using UnityEngine;
using System.Collections.Generic;
using XCharts.Runtime;

public class VisualisePopulationGraph : MonoBehaviour
{
    private void Awake()
    {
        LineChart chart = gameObject.GetComponent<LineChart>();
        if (chart == null)
        {
            Debug.LogWarning("Chart missing! Adding a chart now...");
            CreateGraph(chart);
        }

        chart.RemoveData();

        List<AnimalDataPoint> animalHistory = AnimalAnalytics.Instance.GetAnimalHistory();
        Dictionary<string, int> graphIndexFromAnimalCategory = new Dictionary<string, int>();
        int graphCount = 0;
        int currTime = -1;



        Debug.Log(animalHistory.Count);
        foreach (AnimalDataPoint data in animalHistory) {
            // Since the list is already sorted by time
            if (currTime != data.time)
            {
                currTime = data.time;
                chart.AddXAxisData(currTime.ToString());
            }

            string animalCategory = data.animalType; // in the future can group similar animals together

            if (!graphIndexFromAnimalCategory.ContainsKey(animalCategory))
            {
                graphIndexFromAnimalCategory[animalCategory] = graphCount;
                chart.AddSerie<Line>(animalCategory);
                graphCount++;
            }

            int index = graphIndexFromAnimalCategory[animalCategory];
            chart.AddData(index, data.count);
            
        }
    }

    private void Update()
    {
        
    }

    private void CreateGraph(LineChart chart)
    {
        // Fall back
        chart = gameObject.AddComponent<LineChart>();
        chart.Init();
        chart.SetSize(1200, 700);

        var title = chart.EnsureChartComponent<Title>();
        title.text = "Population Over Time";

        // todo: continue customising (not the focus now, as the graph should already exist

        // add legend
        // set x-axis and y-axis title
        // remove x-axis ticks
    }
}

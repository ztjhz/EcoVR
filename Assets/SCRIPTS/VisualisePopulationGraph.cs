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
        HashSet<string> animalAdded = new HashSet<string>();
        int graphCount = 0;
        int currTime = -1;
        int currDataIndex = -1;

        HashSet<string> temp = new HashSet<string> { };

        foreach (AnimalDataPoint data in animalHistory) {
            // Since the list is already sorted by time
            if (currTime != data.time)
            {
                currTime = data.time;
                chart.AddXAxisData(currTime.ToString());
                currDataIndex += 1;
                animalAdded.Clear();
            }

            // Group all the same species together (e.g. Deer_v4 and Deer_v5)
            string animalCategory = AnimalAnalytics.CleanAnimalName(data.animalName);

            temp.Add(animalCategory);

            if (!graphIndexFromAnimalCategory.ContainsKey(animalCategory))
            {
                graphIndexFromAnimalCategory[animalCategory] = graphCount;
                Line serie = chart.AddSerie<Line>(animalCategory);

                if (data.animalType == "prey")
                    serie.symbol.type = SymbolType.EmptyCircle;
                else
                {
                    serie.symbol.type = SymbolType.EmptyTriangle;
                    serie.EnsureComponent<AreaStyle>();
                }
                serie.symbol.size = 8;
                graphCount++;
            }

            int index = graphIndexFromAnimalCategory[animalCategory];
            if (animalAdded.Contains(animalCategory))
            {
                double currCount = chart.GetData(index, currDataIndex);
                chart.UpdateData(index, currDataIndex, currCount + data.count);
            }
            else
            {
                chart.AddData(index, data.count);
                animalAdded.Add(animalCategory);
            }
        }

        foreach (string x in temp)
            Debug.Log(x);
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

using UnityEngine;

public class TabManager : MonoBehaviour
{
    public GameObject AnimalCountsPanel;  // Panel for the Animal Counts
    public GameObject PopulationGraphPanel;  // Panel for the Animal Population Graph
    public GameObject HeatMapPanel; // Panel for the Animal Heatmaps

    public GameObject ProjectionPanel; // Panel for Animal Population Projections
    public GameObject AnimalAttributesPanel; // Panel for Animal Attributes Projections

    // These are the tab buttons
    public GameObject AnimalCountsTab;
    public GameObject PopulationGraphTab;
    public GameObject HeatMapTab;
    public GameObject ProjectionTab;
    public GameObject AnimalAttributesTab;

    public void SwitchToAnimalCounts()
    {
        Debug.Log("Switching to " + AnimalCountsTab.name);


        AnimalCountsPanel.SetActive(true);
        PopulationGraphPanel.SetActive(false);
        HeatMapPanel.SetActive(false);
        ProjectionPanel.SetActive(false);
        AnimalAttributesPanel.SetActive(false);


        AnimalCountsTab.SetActive(true);
        PopulationGraphTab.SetActive(true);
        HeatMapTab.SetActive(true);
        ProjectionTab.SetActive(true);
        AnimalAttributesTab.SetActive(true);
    }

    public void SwitchToPopulationGraph()
    {
        Debug.Log("Switching to " + PopulationGraphTab.name);


        AnimalCountsPanel.SetActive(false);
        PopulationGraphPanel.SetActive(true);
        HeatMapPanel.SetActive(false);
        ProjectionPanel.SetActive(false);
        AnimalAttributesPanel.SetActive(false);


        AnimalCountsTab.SetActive(true);
        PopulationGraphTab.SetActive(true);
        HeatMapTab.SetActive(true);
        ProjectionTab.SetActive(true);
        AnimalAttributesTab.SetActive(true);
    }

    public void SwitchtoHeatMaps()
    {
        Debug.Log("Switching to " + HeatMapTab.name);


        AnimalCountsPanel.SetActive(false);
        PopulationGraphPanel.SetActive(false);
        HeatMapPanel.SetActive(true);
        ProjectionPanel.SetActive(false);
        AnimalAttributesPanel.SetActive(false);


        AnimalCountsTab.SetActive(true);
        PopulationGraphTab.SetActive(true);
        HeatMapTab.SetActive(true);
        ProjectionTab.SetActive(true);
        AnimalAttributesTab.SetActive(true);
    }

    public void SwitchToProjections()
    {
        Debug.Log("Switching to " + ProjectionTab.name);


        AnimalCountsPanel.SetActive(false);
        PopulationGraphPanel.SetActive(false);
        HeatMapPanel.SetActive(false);
        ProjectionPanel.SetActive(true);
        AnimalAttributesPanel.SetActive(false);


        AnimalCountsTab.SetActive(true);
        PopulationGraphTab.SetActive(true);
        HeatMapTab.SetActive(true);
        ProjectionTab.SetActive(true);
        AnimalAttributesTab.SetActive(true);
    }

    public void SwitchToAnimalAttributes()
    {
        Debug.Log("Switching to " + AnimalAttributesTab.name);


        AnimalCountsPanel.SetActive(false);
        PopulationGraphPanel.SetActive(false);
        HeatMapPanel.SetActive(false);
        ProjectionPanel.SetActive(false);
        AnimalAttributesPanel.SetActive(true);


        AnimalCountsTab.SetActive(true);
        PopulationGraphTab.SetActive(true);
        HeatMapTab.SetActive(true);
        ProjectionTab.SetActive(true);
        AnimalAttributesTab.SetActive(true);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject[] preyPrefabs;
    public GameObject[] predatorPrefabs;
    public int initialPreyCount = 5;
    public int initialPredatorCount = 2;
    public int maxAnimals = 50;
    public float spawnRange = 10f;
    public float checkInterval = 10f; // Check every 10 seconds
    
    private int currentPreyCount = 0;
    private int currentPredatorCount = 0;
    private List<GameObject> activePrey = new List<GameObject>();
    private List<GameObject> activePredators = new List<GameObject>();
    
    public Button spawnPreyButton;
    public Button spawnPredatorButton;

    void Start()
    {
        if (preyPrefabs.Length == 0 || predatorPrefabs.Length == 0)
        {
            Debug.LogError("No animal prefabs assigned!");
            return;
        }

        if (spawnPreyButton != null)
            spawnPreyButton.onClick.AddListener(SpawnPrey);
        else
            Debug.LogError("Spawn Prey Button not assigned!");

        if (spawnPredatorButton != null)
            spawnPredatorButton.onClick.AddListener(SpawnPredator);
        else
            Debug.LogError("Spawn Predator Button not assigned!");

        SpawnInitialAnimals();
        InvokeRepeating("EvaluatePopulation", checkInterval, checkInterval);
    }

    void SpawnInitialAnimals()
    {
        for (int i = 0; i < initialPreyCount; i++)
            SpawnPreyInternal();

        for (int i = 0; i < initialPredatorCount; i++)
            SpawnPredatorInternal();
    }

    public void SpawnPrey()
    {
        if (currentPreyCount < maxAnimals)
            SpawnPreyInternal();
    }

    public void SpawnPrey(GameObject prey)
    {
        if (currentPreyCount >= maxAnimals) return;

        SpawnPreyInternal(prey);
    }

    private void SpawnPreyInternal()
    {
        // Random prey
        GameObject preyPrefab = preyPrefabs[Random.Range(0, preyPrefabs.Length)];
        SpawnPreyInternal(preyPrefab);
    }

    private void SpawnPreyInternal(GameObject preyPrefab)
    {
        if (preyPrefab == null) return;

        Vector3 spawnPosition = GetSpawnPosition();
        GameObject newPrey = Instantiate(preyPrefab, spawnPosition, Quaternion.identity);
        activePrey.Add(newPrey);
        currentPreyCount++;
        newPrey.tag = "prey";
    }

    public void SpawnPredator()
    {
        if (currentPredatorCount < maxAnimals)
            SpawnPredatorInternal();
        else
            Debug.Log("Max Predator Limit Reached!");
    }

    public void SpawnPredator(GameObject predator)
    {
        if (currentPredatorCount >= maxAnimals) return;

        SpawnPredatorInternal(predator);
    }

    private void SpawnPredatorInternal()
    {
        GameObject predatorPrefab = predatorPrefabs[Random.Range(0, predatorPrefabs.Length)];
        SpawnPredatorInternal(predatorPrefab);
    }

    private void SpawnPredatorInternal(GameObject predatorPrefab)
    {
        Vector3 spawnPosition = GetSpawnPosition();
        GameObject newPredator = Instantiate(predatorPrefab, spawnPosition, Quaternion.identity);
        activePredators.Add(newPredator);
        currentPredatorCount++;
        newPredator.tag = "predator";
    }

    Vector3 GetSpawnPosition()
    {
        Vector3 randomSpawnPosition = new Vector3(
            transform.position.x + Random.Range(-spawnRange, spawnRange),
            1000f,
            transform.position.z + Random.Range(-spawnRange, spawnRange)
        );

        RaycastHit hit;
        if (Physics.Raycast(randomSpawnPosition, Vector3.down, out hit))
        {
            UnityEngine.AI.NavMeshHit navHit;
            if (UnityEngine.AI.NavMesh.SamplePosition(hit.point, out navHit, 10f, UnityEngine.AI.NavMesh.AllAreas))
                return navHit.position;
        }

        Debug.LogWarning("Failed to find valid NavMesh position, using fallback.");
        return transform.position;
    }

    public void DecrementPreyCount(GameObject prey)
    {
        if (activePrey.Contains(prey))
        {
            activePrey.Remove(prey);
            Destroy(prey);
            currentPreyCount = Mathf.Max(0, currentPreyCount - 1);
        }
    }

    public void DecrementPredatorCount(GameObject predator)
    {
        if (activePredators.Contains(predator))
        {
            activePredators.Remove(predator);
            Destroy(predator);
            currentPredatorCount = Mathf.Max(0, currentPredatorCount - 1);
        }
    }

    void EvaluatePopulation()
    {
        activePrey.RemoveAll(item => item == null);
        activePredators.RemoveAll(item => item == null);
        currentPreyCount = activePrey.Count;
        currentPredatorCount = activePredators.Count;
    }
}
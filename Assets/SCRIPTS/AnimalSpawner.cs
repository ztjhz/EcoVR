using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class AnimalSpawner : MonoBehaviour
{
    public GameObject[] preyPrefabs;
    public GameObject[] predatorPrefabs;
    public int initialPreyCount = 5; // Initial number of prey at start
    public int initialPredatorCount = 2; // Initial number of predators at start
    public int maxAnimals = 50;
    public float spawnRange = 10f;

    private int currentPreyCount = 0;
    private int currentPredatorCount = 0;

    // UI Buttons
    public Button spawnPreyButton;
    public Button spawnPredatorButton;

    void Start()
    {
        if (preyPrefabs.Length == 0 || predatorPrefabs.Length == 0)
        {
            Debug.LogError("No animal prefabs assigned!");
            return;
        }

        // Setup Button Listeners
        if (spawnPreyButton != null)
        {
            spawnPreyButton.onClick.AddListener(SpawnPrey);
        }
        else
        {
            Debug.LogError("Spawn Prey Button not assigned!");
        }

        if (spawnPredatorButton != null)
        {
            spawnPredatorButton.onClick.AddListener(SpawnPredator);
        }
        else
        {
            Debug.LogError("Spawn Predator Button not assigned!");
        }

        // Spawn Initial Animals
        SpawnInitialAnimals();
    }

    void SpawnInitialAnimals()
    {
        for (int i = 0; i < initialPreyCount; i++)
        {
            SpawnPreyInternal(); // Use internal spawn function
        }

        for (int i = 0; i < initialPredatorCount; i++)
        {
            SpawnPredatorInternal(); // Use internal spawn function
        }
    }

    // Spawns a random prey animal (called from UI button)
    public void SpawnPrey()
    {
        if (currentPreyCount < maxAnimals)
        {
            SpawnPreyInternal(); // Call the internal method
        }
        else
        {
            Debug.Log("Max Prey Limit Reached!");
        }
    }

    // Internal method for spawning prey (used by both initial spawn and button)
    private void SpawnPreyInternal()
    {
        GameObject preyPrefab = preyPrefabs[Random.Range(0, preyPrefabs.Length)];
        Vector3 spawnPosition = GetSpawnPosition();
        GameObject newAnimal = Instantiate(preyPrefab, spawnPosition, Quaternion.identity);
        currentPreyCount++;
        // Assign tag for easier tracking.
        newAnimal.tag = "prey";
    }

    // Spawns a random predator animal (called from UI button)
    public void SpawnPredator()
    {
        if (currentPredatorCount < maxAnimals)
        {
            SpawnPredatorInternal(); // Call the internal method
        }
        else
        {
            Debug.Log("Max Predator Limit Reached!");
        }
    }

    private void SpawnPredatorInternal()
    {
        GameObject predatorPrefab = predatorPrefabs[Random.Range(0, predatorPrefabs.Length)];
        Vector3 spawnPosition = GetSpawnPosition();
        GameObject newAnimal = Instantiate(predatorPrefab, spawnPosition, Quaternion.identity);
        currentPredatorCount++;
        // Assign tag for easier tracking.
        newAnimal.tag = "predator";
    }

    // Calculates spawn position
    Vector3 GetSpawnPosition()
    {
        Vector3 spawnPosition = new Vector3(
            transform.position.x + Random.Range(-spawnRange, spawnRange),
            0f,
            transform.position.z + Random.Range(-spawnRange, spawnRange)
        );

        return spawnPosition;
    }

    // Method to decrement prey count (called from PreyAI)
    public void DecrementPreyCount()
    {
        currentPreyCount = Mathf.Max(0, currentPreyCount - 1);
    }

    // Method to decrement predator count (called from PredatorAI)
    public void DecrementPredatorCount()
    {
        currentPredatorCount = Mathf.Max(0, currentPredatorCount - 1);
    }
}
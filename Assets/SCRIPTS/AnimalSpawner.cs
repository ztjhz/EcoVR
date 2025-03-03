using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject[] animalPrefabs;  // Array of animal prefabs to spawn
    public Camera mainCamera;           // Reference to the camera
    public int numberOfAnimals = 20;     // Number of animals to spawn
    public float spawnRange = 10f;      // Range within which to spawn animals

    void Start()
    {
        if (animalPrefabs.Length == 0)
        {
            Debug.LogError("No animal prefabs assigned!");
            return;
        }

        if (mainCamera == null)
        {
            Debug.LogError("No camera assigned!");
            return;
        }

        // Spawn animals at runtime
        SpawnAnimals();
    }

    void SpawnAnimals()
    {
        for (int i = 0; i < numberOfAnimals; i++)
        {
            // Choose a random animal prefab from the array
            GameObject animalPrefab = animalPrefabs[Random.Range(0, animalPrefabs.Length)];
            
            // Keep track of animal count for analytics
            AnimalAnalytics.Instance.AddAnimal(animalPrefab.name);

            // Generate a random spawn position around the camera (ignore the y value for now)
            Vector3 spawnPosition = new Vector3(
                mainCamera.transform.position.x + Random.Range(-spawnRange, spawnRange),
                1000f, // Set an arbitrarily large y value to ensure the raycast hits the ground
                mainCamera.transform.position.z + Random.Range(-spawnRange, spawnRange)
            );

            // Raycast downwards to find the ground's position
            RaycastHit hit;
            if (Physics.Raycast(spawnPosition, Vector3.down, out hit))
            {
                // Set the spawn position to the ground's y value
                spawnPosition.y = hit.point.y; // hit.point gives the position where the ray hits the ground
            }

            // Instantiate the animal prefab at the calculated position
            Instantiate(animalPrefab, spawnPosition, Quaternion.identity);
        }
    }

}
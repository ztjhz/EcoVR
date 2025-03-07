using UnityEngine;
using System.Collections.Generic;

public class ConwayPopulationGrowth : MonoBehaviour
{
    public int gridSize = 600; // 600x600 coordinate-based grid
    public int cellSize = 20;  // Each cell represents a 20x20 area
    private int[,] grid;
    private int numCells;
    private Vector3 gridCenter;
    [SerializeField] private List<GameObject> preyPrefabs; // List of prey prefabs
    [SerializeField] private List<GameObject> predatorPrefabs; // List of predator prefabs
    private AnimalSpawner spawner;
    
    void Start()
    {
        spawner = FindObjectOfType<AnimalSpawner>();
        if (spawner == null)
            Debug.LogError("AnimalSpawner not found in the scene!");
        
        numCells = gridSize / cellSize;
        grid = new int[numCells, numCells];
        gridCenter = Camera.main.transform.position; // Set grid center to the main camera
        InitializeGrid();
    }

    void Update()
    {
        if (Time.frameCount % 30 == 0) // Update every 30 frames (~every half-second)
        {
            UpdateGameOfLife();
        }
    }

    void InitializeGrid()
    {
        for (int x = 0; x < numCells; x++)
        {
            for (int y = 0; y < numCells; y++)
            {
                grid[x, y] = Random.Range(0, 2); // Randomly populate with prey (1) or empty (0)
            }
        }
    }

    void UpdateGameOfLife()
    {
        int[,] newGrid = new int[numCells, numCells];

        for (int x = 0; x < numCells; x++)
        {
            for (int y = 0; y < numCells; y++)
            {
                int neighbors = CountNeighbors(x, y);
                
                if (grid[x, y] == 1) // If prey exists
                {
                    if (neighbors < 2 || neighbors > 3)
                        newGrid[x, y] = 0; // Prey dies
                    else
                        newGrid[x, y] = 1; // Prey survives
                }
                else // If empty
                {
                    if (neighbors == 3)
                        newGrid[x, y] = 1; // New prey is born
                }
            }
        }

        ApplyGridChanges(newGrid);
        grid = newGrid;
    }

    int CountNeighbors(int x, int y)
    {
        int count = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                int nx = x + i, ny = y + j;
                if (nx >= 0 && nx < numCells && ny >= 0 && ny < numCells)
                    count += grid[nx, ny];
            }
        }
        return count;
    }

    void ApplyGridChanges(int[,] newGrid)
    {
        for (int x = 0; x < numCells; x++)
        {
            for (int y = 0; y < numCells; y++)
            {
                Vector3 worldPos = GridToWorld(x, y);

                if (grid[x, y] == 0 && newGrid[x, y] == 1)
                {
                    // Use existing SpawnPrey method without prefab selection
                    spawner.SpawnPrey();
                }
                else if (grid[x, y] == 1 && newGrid[x, y] == 0)
                {
                    PreyAI prey = FindPreyAt(worldPos);
                    if (prey)
                        prey.Die();
                }
            }
        }
    }


    Vector3 GridToWorld(int x, int y)
    {
        float worldX = gridCenter.x - gridSize / 2 + x * cellSize;
        float worldY = gridCenter.z - gridSize / 2 + y * cellSize;
        return new Vector3(worldX, 0, worldY);
    }

    PreyAI FindPreyAt(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, cellSize / 2);
        foreach (Collider col in colliders)
        {
            PreyAI prey = col.GetComponent<PreyAI>();
            if (prey) return prey;
        }
        return null;
    }
}

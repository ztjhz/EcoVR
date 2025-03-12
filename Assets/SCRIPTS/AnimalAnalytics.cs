using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Linq;
using System;

public class AnimalAnalytics : MonoBehaviour
{
    public static AnimalAnalytics Instance { get; private set; }

    private Dictionary<string, int> animalCounts = new Dictionary<string, int>();
    private Dictionary<string, List<Vector3>> animalPositions = new Dictionary<string, List<Vector3>>();
    private List<AnimalDataPoint> animalHistory = new List<AnimalDataPoint>();
    
    private float timeElapsed = 0f;
    private float logIntervalInSeconds = 5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make it persist across scenes.
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= logIntervalInSeconds)
        {
            TrackAllAnimals();
            LogAnimalData();
            timeElapsed = 0f;
        }
    }

    public void TrackAllAnimals()
    {
        GameObject[] preys = GameObject.FindGameObjectsWithTag("prey");
        GameObject[] predators = GameObject.FindGameObjectsWithTag("predator");
        GameObject[] animals = new GameObject[preys.Length + predators.Length];

        Array.Copy(preys, 0, animals, 0, preys.Length);
        Array.Copy(predators, 0, animals, preys.Length, predators.Length);

        animalCounts = animalCounts.ToDictionary(kvp => kvp.Key, kvp => 0);
        animalPositions = animalPositions.ToDictionary(kvp => kvp.Key, kvp => new List<Vector3>());

        foreach (GameObject animal in animals)
        {
            string animalType = animal.name.Replace("(Clone)", "").Trim();
            Vector3 position = animal.transform.position;

            // Count animals
            if (!animalCounts.ContainsKey(animalType))
                animalCounts[animalType] = 0;
            animalCounts[animalType]++;

            // Store positions
            if (!animalPositions.ContainsKey(animalType))
                animalPositions[animalType] = new List<Vector3>();
            animalPositions[animalType].Add(position);
        }
    }

    private void LogAnimalData()
    {
        int currTime = (int)Math.Round(Time.time);
        foreach (var animal in animalCounts)
        {
            List<Vector3> positions = animalPositions[animal.Key];
            string positionsString = string.Join(", ", positions.Select(p => $"({p.x:F2}, {p.y:F2}, {p.z:F2})"));
            animalHistory.Add(new AnimalDataPoint(currTime, animal.Key, animal.Value, positions));
            Debug.Log($"[LOG] Time: {currTime}, {animal.Key} Count: {animal.Value}, Positions {positionsString}");
        }
    }

    public int GetAnimalCount(string animalType)
    {
        return animalCounts.ContainsKey(animalType) ? animalCounts[animalType] : 0;
    }

    public List<Vector3> GetAnimalPositions(string animalType)
    {
        return animalPositions.ContainsKey(animalType) ? animalPositions[animalType] : new List<Vector3>();
    }

    public List<AnimalDataPoint> GetAnimalHistory()
    {
        return animalHistory;
    }
}

[System.Serializable]
public class AnimalDataPoint
{
    public int time;
    public string animalType;
    public int count;
    public List<Vector3> positions;

    public AnimalDataPoint(int time, string animalType, int count, List<Vector3> positions)
    {
        this.time = time;
        this.animalType = animalType;
        this.count = count;
        this.positions = positions;
    }
}

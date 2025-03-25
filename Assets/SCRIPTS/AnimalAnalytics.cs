using UnityEngine;
using Ursaanimation.CubicFarmAnimals;
using System.Collections.Generic;
using System.Linq;
using System;

public class AnimalAnalytics : MonoBehaviour
{
    public static AnimalAnalytics Instance { get; private set; }

    private Dictionary<string, int> animalCounts = new Dictionary<string, int>();
    private Dictionary<string, List<Vector3>> animalPositions = new Dictionary<string, List<Vector3>>();
    private Dictionary<string, string> animalTypes = new Dictionary<string, string>();
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
            string animalName = animal.name.Replace("(Clone)", "").Trim();
            animalName = CleanAnimalName(animalName);
            Vector3 position = animal.transform.position;

            Debug.Log($"Tracking animal: {animalName}");

            // Count animals
            if (!animalCounts.ContainsKey(animalName))
                animalCounts[animalName] = 0;
            animalCounts[animalName]++;

            // Store positions
            if (!animalPositions.ContainsKey(animalName))
                animalPositions[animalName] = new List<Vector3>();
            animalPositions[animalName].Add(position);

            // Store animal type
            if (!animalTypes.ContainsKey(animalName))
                animalTypes[animalName] = animal.tag;
        }
    }

    private void LogAnimalData()
    {
        int currTime = (int)Math.Round(Time.time);
        foreach (var animal in animalCounts)
        {
            List<Vector3> positions = animalPositions[animal.Key];
            string positionsString = string.Join(", ", positions.Select(p => $"({p.x:F2}, {p.y:F2}, {p.z:F2})"));
            animalHistory.Add(new AnimalDataPoint(currTime, animal.Key, animalTypes[animal.Key], animal.Value, positions));
            Debug.Log($"[LOG] Time: {currTime}, {animal.Key} Count: {animal.Value}, Positions {positionsString}");
        }
    }

    public static string CleanAnimalName(string name)
    {
        // Convert animal name to root form (e.g. SK_Goat_white -> Goat)
        string[] parts = name.Split('_');

        if (parts.Length == 2)
            return parts[0];
        if (parts.Length == 3)
            return parts[1];

        return name;
    }

    public Dictionary<string, int> GetAnimalCount()
    {
        return animalCounts;
    }

    public List<Vector3> GetAnimalPositions(string animalType)
    {
        return animalPositions.ContainsKey(animalType) ? animalPositions[animalType] : new List<Vector3>();
    }

    public string GetAnimalType(string animalName)
    {
        string cleanedName = CleanAnimalName(animalName);

        foreach (var kvp in animalTypes)
        {
            if (CleanAnimalName(kvp.Key) == cleanedName)
            {
                return kvp.Value;
            }
        }

        Debug.LogWarning($"Animal type for {animalName} not found!");
        return "Unknown";
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
    public string animalName;
    public string animalType; // prey or predator
    public int count;
    public List<Vector3> positions;

    public AnimalDataPoint(int time, string animalName, string animalType, int count, List<Vector3> positions)
    {
        this.time = time;
        this.animalName = animalName;
        this.animalType = animalType;
        this.count = count;
        this.positions = positions;
    }

    public override string ToString()
    {
        return $"[Time: {time}, Name: {animalName}, Type: {animalType}, Count: {count}]";
    }
}

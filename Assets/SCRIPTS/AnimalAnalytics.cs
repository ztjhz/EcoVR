using UnityEngine;
using System.Collections.Generic;

public class AnimalAnalytics : MonoBehaviour
{
    public static AnimalAnalytics Instance { get; private set; }

    private Dictionary<string, int> animalCounts = new Dictionary<string, int>();
    private List<AnimalDataPoint> animalHistory = new List<AnimalDataPoint>();
    private float timeElapsed = 0f;
    private float logInterval = 1f; // Log every 1 second

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
        if (timeElapsed >= logInterval)
        {
            LogAnimalData();
            timeElapsed = 0f;
        }
    }

    public void AddAnimal(string animalType)
    {
        if (!animalCounts.ContainsKey(animalType))
            animalCounts[animalType] = 0;

        animalCounts[animalType]++;
        Debug.Log($"[ADD] {animalType} added. Total: {animalCounts[animalType]}");
    }

    public void RemoveAnimal(string animalType)
    {
        if (animalCounts.ContainsKey(animalType) && animalCounts[animalType] > 0)
            animalCounts[animalType]--;
    }

    public int GetAnimalCount(string animalType)
    {
        return animalCounts.ContainsKey(animalType) ? animalCounts[animalType] : 0;
    }

    private void LogAnimalData()
    {
        foreach (var animal in animalCounts)
        {
            animalHistory.Add(new AnimalDataPoint(Time.time, animal.Key, animal.Value));
            Debug.Log($"[LOG] Time: {Time.time}, {animal.Key} Count: {animal.Value}");
        }
    }

    public List<AnimalDataPoint> GetAnimalHistory()
    {
        return animalHistory;
    }
}

[System.Serializable]
public class AnimalDataPoint
{
    public float time;
    public string animalType;
    public int count;

    public AnimalDataPoint(float time, string animalType, int count)
    {
        this.time = time;
        this.animalType = animalType;
        this.count = count;
    }
}

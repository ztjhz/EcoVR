using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalNeedsManager : MonoBehaviour
{
    public enum WeatherType { Normal, Sunny, Stormy, Snowy, Foggy }

    public static AnimalNeedsManager Instance { get; private set; }

    private List<IAnimalStatus> trackedAnimals = new List<IAnimalStatus>();
    private WeatherType currentWeather = WeatherType.Normal;

    private float updateInterval = 20f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(UpdateAnimalNeeds());
    }

    public void RegisterAnimal(IAnimalStatus animal)
    {
        if (!trackedAnimals.Contains(animal))
        {
            trackedAnimals.Add(animal);
        }
    }

    public void UnregisterAnimal(IAnimalStatus animal)
    {
        trackedAnimals.Remove(animal);
    }

    public void SetWeather(WeatherType newWeather)
    {
        currentWeather = newWeather;
        StopAllCoroutines();
        StartCoroutine(UpdateAnimalNeeds());
    }

    private IEnumerator UpdateAnimalNeeds()
    {
        while (true)
        {
            float interval = updateInterval;

            foreach (var animal in trackedAnimals)
            {
                switch (currentWeather)
                {
                    case WeatherType.Sunny:
                        animal.DecreaseHydration();
                        interval = 10f;
                        break;

                    case WeatherType.Stormy:
                        animal.IncreaseHydration();
                        animal.ModifyHuntingRadius(0.5f);
                        interval = 10f;
                        break;

                    case WeatherType.Foggy:
                        animal.ModifyHuntingRadius(0.4f);
                        break;

                    case WeatherType.Snowy:
                        animal.DecreaseHydration();
                        animal.DecreaseFullness();
                        animal.ModifyHuntingRadius(0.7f);
                        break;

                    default:
                        animal.DecreaseHydration();

                        if (animal.IsPredator())
                        {
                            animal.DecreaseFullness();
                        }
                        break;
                }
            }

            yield return new WaitForSeconds(interval);
        }
    }
}

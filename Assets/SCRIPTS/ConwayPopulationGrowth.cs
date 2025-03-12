using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ConwayPopulationGrowth : MonoBehaviour
{
    //[SerializeField] private List<GameObject> preyPrefabs;
    //[SerializeField] private List<GameObject> predatorPrefabs;
    private AnimalSpawner spawner;
    private float timeElapsed = 0f;
    private float updateIntervalInSeconds = 10f;

    // Death controls
    // [Overpopulation] Preys compete for the same food (grass) in an area
    private float preyConsumptionRadius = 100;
    private int preyCompetitionThreshold = 10;

    // [Overpopulation] Predators compete for the same food (prey) in an area
    private float predatorConsumptionRadius = 300;
    private int predatorCompetitionThreshold = 5;

    // [Underpopulation] If animals are too isolated, they may feel lonely or cannot survive on their own
    private float companionRadius = 100;
    private int companionFloor = 2; // need at least 1 companion including itself
    private float deathByLonelinessProb = 0.8f;

    // Birth controls
    // [Reproduction]
    private float reproductionRadius = 50; // how close animials must be to reproduce
    private int reproductionFloor = 2; // how many animals are needed to reproduce
    private float reproductionProb = 0.5f; // how likely animals will reproduce if together


    void Start()
    {
        spawner = FindObjectOfType<AnimalSpawner>();
        if (spawner == null)
            Debug.LogError("AnimalSpawner not found in the scene!");
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= updateIntervalInSeconds)
        {
            UpdateGameOfLife();
            timeElapsed = 0f;
        }
    }

    void UpdateGameOfLife()
    {
        Debug.Log("[LOG] Updating Game of Life...");

        //GameObject[] allPrey = GameObject.FindObjectsOfType<GameObject>().Where(obj => preyPrefabs.Contains(obj)).ToArray();
        //GameObject[] allPredator = GameObject.FindObjectsOfType<GameObject>().Where(obj => predatorPrefabs.Contains(obj)).ToArray();
        GameObject[] allPrey = GameObject.FindGameObjectsWithTag("prey");
        GameObject[] allPredator = GameObject.FindGameObjectsWithTag("predator");

        Debug.Log($"[DEBUG] prey count {allPrey.Length}, predator count {allPredator.Length}");

        foreach (GameObject prey in allPrey)
        {
            int companionCount = CountCompanions(prey);
            int mateCount = CountReproductionMates(prey);
            int competitorCount = CountCompetitions(prey, preyConsumptionRadius);

            string preyName = CleanName(prey.name);

            Debug.Log($"[DEBUG] {preyName} - companion: {companionCount} - mate: {mateCount} - competitor: {competitorCount}");

            // too lonely
            if (companionCount < companionFloor && Random.value < deathByLonelinessProb)
            {
                spawner.DecrementPreyCount(prey);
                Debug.Log($"[LOG] {preyName} died of loneliness.");
            }

            // too much competition
            if (competitorCount > preyCompetitionThreshold)
            {
                spawner.DecrementPreyCount(prey);
                Debug.Log($"[LOG] {preyName} died of overpopulation.");
            }

            // reproduce
            if (mateCount > reproductionFloor && Random.value < reproductionProb)
            {
                spawner.SpawnPrey(prey);
                Debug.Log($"[LOG] {preyName} reproduced!");
            }
        }


        foreach (GameObject predator in allPredator)
        {
            int companionCount = CountCompanions(predator);
            int mateCount = CountReproductionMates(predator);
            int competitorCount = CountCompetitions(predator, predatorConsumptionRadius);

            string predatorName = CleanName(predator.name);

            Debug.Log($"[DEBUG] {predatorName} - companion: {companionCount} - mate: {mateCount} - competitor: {competitorCount}");

            // too lonely
            if (companionCount < companionFloor && Random.value < deathByLonelinessProb)
            {
                spawner.DecrementPredatorCount(predator);
                Debug.Log($"[LOG] {predatorName} died of loneliness.");
            }

            // too much competition
            if (competitorCount > predatorCompetitionThreshold)
            {
                spawner.DecrementPredatorCount(predator);
                Debug.Log($"[LOG] {predatorName} died of overpopulation.");
            }

            // reproduce
            if (mateCount > reproductionFloor && Random.value < reproductionProb)
            {
                spawner.SpawnPredator(predator);
                Debug.Log($"[LOG] {predatorName} reproduced!");
            }
        }
        Debug.Log("[LOG] Game of Life updated!");
    }

    private int CountCompanions(GameObject animal)
    {
        Collider[] companions = Physics.OverlapSphere(animal.transform.position, companionRadius)
            .Where(p => p.tag == animal.tag)
            .ToArray();
        return companions.Length;
    }

    private int CountCompetitions(GameObject animal, float competitionRadius)
    {
        Collider[] competitors = Physics.OverlapSphere(animal.transform.position, competitionRadius)
            .Where(p => p.tag == animal.tag)
            .ToArray();
        return competitors.Length;
    }

    private int CountReproductionMates(GameObject animal)
    {
        Collider[] mates = Physics.OverlapSphere(animal.transform.position, reproductionRadius)
            .Where(p => CleanName(p.name) == CleanName(animal.name))
            .ToArray();
        return mates.Length;
    }

    private string CleanName(string name)
    {
        return AnimalAnalytics.CleanAnimalName(name.Replace("(Clone)", "").Trim());
    }
}

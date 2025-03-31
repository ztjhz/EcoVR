using UnityEngine;

public class HandleConsumption : MonoBehaviour
{
    private bool canBeFed = true;
    private float feedCooldown = 5.0f;

    private IAnimalStatus animalStatus;

    private void Start()
    {
        animalStatus = GetComponent<IAnimalStatus>();

        if (animalStatus == null)
        {
            Debug.LogWarning("HandleConsumption: No IAnimalStatus found on this object!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canBeFed || animalStatus == null) return;

        if (other.CompareTag("Food"))
        {
            FeedFood(other.gameObject);
        }
        else if (other.CompareTag("Water"))
        {
            FeedWater(other.gameObject);
        }
    }

    private void FeedFood(GameObject food)
    {
        canBeFed = false;
        food.SetActive(false);
        animalStatus.IncreaseFullness();
        Debug.Log("Fed food: Fullness increased!");
        Invoke(nameof(ResetFeedCooldown), feedCooldown);
    }

    private void FeedWater(GameObject water)
    {
        canBeFed = false;
        water.SetActive(false);
        animalStatus.IncreaseHydration();
        Debug.Log("Fed water: Hydration increased!");
        Invoke(nameof(ResetFeedCooldown), feedCooldown);
    }

    private void ResetFeedCooldown()
    {
        canBeFed = true;
    }
}

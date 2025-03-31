using UnityEngine;

public class HandleConsumption : MonoBehaviour
{
    private bool canBeFed = true;
    private float feedCooldown = 5.0f;


    private void OnTriggerEnter(Collider other)
    {
        if (canBeFed && other.CompareTag("Food"))
            FeedFood(other.gameObject);
        if (canBeFed && other.CompareTag("Water"))
            FeedWater(other.gameObject);
    }

    private void FeedFood(GameObject food)
    {
        if (!canBeFed) return;

        canBeFed = false;
        food.SetActive(false);

        Invoke(nameof(ResetFeedCooldown), feedCooldown);

        Debug.Log("Feed food!");
    }

    private void FeedWater(GameObject water)
    {
        if (!canBeFed) return;

        canBeFed = false;
        water.SetActive(false);

        Invoke(nameof(ResetFeedCooldown), feedCooldown);

        Debug.Log("Feed water!");
    }

    private void ResetFeedCooldown()
    {
        canBeFed = true;
    }
}
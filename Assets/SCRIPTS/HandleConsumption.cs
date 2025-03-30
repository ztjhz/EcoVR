using UnityEngine;

public class HandleConsumption : MonoBehaviour
{
    private bool canBeFed = true;
    private float feedCooldown = 5.0f;


    private void OnTriggerEnter(Collider other)
    {
        if (canBeFed && other.CompareTag("Food"))
            FeedFood();
        if (canBeFed && other.CompareTag("Water"))
            FeedWater();
    }

    private void FeedFood()
    {
        if (!canBeFed) return;

        canBeFed = false;

        Invoke(nameof(ResetFeedCooldown), feedCooldown);

        Debug.Log("Feed food!");
    }

    private void FeedWater()
    {
        if (!canBeFed) return;

        canBeFed = false;

        Invoke(nameof(ResetFeedCooldown), feedCooldown);

        Debug.Log("Feed water!");
    }

    private void ResetFeedCooldown()
    {
        canBeFed = true;
    }
}
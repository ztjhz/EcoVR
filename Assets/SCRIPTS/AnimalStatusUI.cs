using UnityEngine;

public class AnimalStatusUI : MonoBehaviour
{
    public ClassicProgressBar hydrationBar;
    public ClassicProgressBar fullnessBar;

    private IAnimalStatus status;

    private void Start()
    {
        status = GetComponent<IAnimalStatus>();

        if (status == null)
        {
            Debug.LogWarning("No IAnimalStatus found on this GameObject!");
            enabled = false;
        }
    }

    void Update()
    {
        if (hydrationBar != null)
        {
            float hydration = GetHydrationLevel();
            hydrationBar.SetFillAmount(hydration / 5f);
        }

        if (fullnessBar != null && status.IsPredator())
        {
            float fullness = GetFullnessLevel();
            fullnessBar.SetFillAmount(fullness / 5f);
        }
    }

    float GetHydrationLevel()
    {
        var field = status.GetType().GetField("hydrationLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return field != null ? (int)field.GetValue(status) : 0;
    }

    float GetFullnessLevel()
    {
        var field = status.GetType().GetField("fullnessLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return field != null ? (int)field.GetValue(status) : 0;
    }
}

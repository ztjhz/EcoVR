using UnityEngine;
using Ursaanimation.CubicFarmAnimals;

public class DetectionRadius : MonoBehaviour
{
    private AnimationController preyAI;
    private PredatorAI predatorAI;
    private float radius;
    private bool isDetectionActive = false;

    private void Start()
    {
        predatorAI = GetComponentInParent<PredatorAI>();
        preyAI = GetComponentInParent<AnimationController>();

        if (predatorAI != null)
            radius = predatorAI.detectionRange;

        if (preyAI != null)
            radius = preyAI.detectionRange;

        SetRadius(radius);
    }

    public void ToggleDetectionRadius()
    {
        isDetectionActive = !isDetectionActive;

        if (isDetectionActive)
            SetRadius(radius);
        else
            SetRadius(0);
    }

    private void SetRadius(float radius)
    {
        Vector3 scale = transform.localScale;

        // Modify the x and z scale to adjust the radius
        scale.x = radius;
        scale.z = radius;

        // Apply the modified scale to the cylinder's transform
        transform.localScale = scale;
    }
}

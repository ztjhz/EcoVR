using UnityEngine;
using Ursaanimation.CubicFarmAnimals;

public class DetectionRadius : MonoBehaviour
{
    private AnimationController preyAI;
    private PredatorAI predatorAI;
    private float radius;
    private InfoToggleManager infoToggleManager;
    private bool isDetectionActive;

    private void Start()
    {
        predatorAI = GetComponentInParent<PredatorAI>();
        preyAI = GetComponentInParent<AnimationController>();
        infoToggleManager = GameObject.Find("InfoToggleManager").GetComponent<InfoToggleManager>();
        isDetectionActive = infoToggleManager.isInfoActive;

        if (predatorAI != null)
            radius = predatorAI.detectionRange;

        if (preyAI != null)
            radius = preyAI.detectionRange;

        if (isDetectionActive)
            SetRadius(radius);
        else
            SetRadius(0);
    }

    private void Update()
    {
        if (isDetectionActive != infoToggleManager.isInfoActive)
        {
            isDetectionActive = infoToggleManager.isInfoActive;

            if (isDetectionActive)
                SetRadius(radius);
            else
                SetRadius(0);
        }

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

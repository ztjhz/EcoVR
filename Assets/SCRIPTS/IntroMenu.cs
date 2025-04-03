using UnityEngine;
using System.Collections;
using TMPro; // If using TextMeshPro

public class IntroMenu : MonoBehaviour
{
    public Camera mainCamera; // Assign Main Camera in Inspector
    public GameObject textDisplay; // Assign "Guide User Text" object
    public float detectRange = 20f; // Distance threshold to start text
    public float blinkRange = 3f; // Distance threshold to start blinking
    public float fastBlinkDuration = 0.01f; // Fast flicker speed

    private bool isTextStarted = false;
    private bool isBlinking = false;
    private bool isFullyVisible = false;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        canvasGroup = GetComponent<CanvasGroup>(); // Get CanvasGroup from Title Panel
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup missing from Title Panel! Please add it.");
            return;
        }

        SetVisibility(false); // Start as invisible
        textDisplay.SetActive(false); // Hide text initially
        StartCoroutine(CheckVisibilityAndDistance());
    }

    IEnumerator CheckVisibilityAndDistance()
    {
        while (!isFullyVisible)
        {
            float distanceToUser = Vector3.Distance(mainCamera.transform.position, transform.position);

            // Check if object is within the camera view
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);
            bool inViewport = viewportPos.x > 0 && viewportPos.x < 1 &&
                              viewportPos.y > 0 && viewportPos.y < 1 &&
                              viewportPos.z > 0; // Ensures it's in front of the camera

            // Start text sequence if object is in view and not already triggered
            if (inViewport && !isTextStarted)
            {
                isTextStarted = true;
                StartCoroutine(ShowTextSequence());
            }

            // Start blinking when the user is close enough
            if (isTextStarted && distanceToUser <= blinkRange && !isBlinking)
            {
                textDisplay.SetActive(false);
                StartCoroutine(BlinkRoutine());  // Start blinking
            }

            yield return new WaitForSeconds(0.5f); // Check every 0.5 seconds
        }
    }

    IEnumerator ShowTextSequence()
    {
        textDisplay.SetActive(true);
        TextMeshProUGUI textComponent = textDisplay.GetComponent<TextMeshProUGUI>();

        string[] messages = { "Hey you.", "Yes. You.", "Interested in the eco-system?", "To learn the evolution of food chain?", "You can find your answers here." };

        foreach (string message in messages)
        {
            textComponent.text = message;
            yield return new WaitForSeconds(3f); // Display each message for 3s
        }

        textDisplay.SetActive(false); // Hide text after last message
    }

    IEnumerator BlinkRoutine()
    {
        isBlinking = true;

        // Fast blinking (0.5 seconds)
        for (int i = 0; i < 5; i++) // 5 times (~0.5 sec)
        {
            ToggleVisibility();
            yield return new WaitForSeconds(fastBlinkDuration);
        }

        // Set permanently visible
        SetVisibility(true);
        isFullyVisible = true;
    }

    void ToggleVisibility()
    {
        canvasGroup.alpha = canvasGroup.alpha == 1 ? 0 : 1;
    }

    void SetVisibility(bool state)
    {
        canvasGroup.alpha = state ? 1 : 0;
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }
}

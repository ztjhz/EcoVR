using UnityEngine;
using Unity.XR.CoreUtils;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera;
    [SerializeField]
    private float proximityThreshold = 5f;

    private Animator doorAnimator;

    private void Start()
    {
        doorAnimator = GetComponent<Animator>();

        if (doorAnimator == null)
        {
            Debug.LogError("Animator component not found on the door object.");
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(m_Camera.transform.position, transform.position);

        if (distance <= proximityThreshold)
        {
            doorAnimator.SetBool("character_nearby", true); // Set CharacterNearby to true to open the door
        }
        else
        {
            doorAnimator.SetBool("character_nearby", false); // Set it back to false to close the door
        }
    }
}
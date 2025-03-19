using UnityEngine;
using UnityEngine.UI;  // For Button

[System.Serializable]
public class XRRigs
{
    public GameObject birdEyeRig;  // The XR rig for bird's-eye view (fixed camera setup)
    public GameObject firstPersonRig;  // The XR rig for first-person view (XR controller enabled)
}

public class XRRigToggle : MonoBehaviour
{
    public XRRigs xrRigs; // Holds the XR rigs (bird's-eye and first-person)
    public Button firstPersonButton;  // Button to toggle to first-person view
    public Button birdEyeButton;  // Button to toggle to bird's-eye view

    void Start()
    {
        // Set button listeners
        if (firstPersonButton != null)
        {
            firstPersonButton.onClick.AddListener(SwitchToFirstPerson); // Button to switch to first-person view
        }

        if (birdEyeButton != null)
        {
            birdEyeButton.onClick.AddListener(SwitchToBirdEye); // Button to switch to bird's-eye view
        }

        // Initially, set the bird's-eye rig as the active rig (default view)
        if (xrRigs.birdEyeRig != null && xrRigs.firstPersonRig != null)
        {
            xrRigs.birdEyeRig.SetActive(true);  // Enable bird's-eye rig
            xrRigs.firstPersonRig.SetActive(false);    // Disable first-person rig
        }
    }

    // Switch to first-person view by manually enabling/disabling the rigs
    public void SwitchToFirstPerson()
    {
        if (xrRigs.birdEyeRig != null && xrRigs.firstPersonRig != null)
        {
            xrRigs.firstPersonRig.SetActive(true);  // Activate first-person rig
            xrRigs.birdEyeRig.SetActive(false);    // Deactivate bird's-eye rig
        }
    }

    // Switch to bird's-eye view by manually enabling/disabling the rigs
    public void SwitchToBirdEye()
    {
        if (xrRigs.birdEyeRig != null && xrRigs.firstPersonRig != null)
        {
            xrRigs.firstPersonRig.SetActive(false); // Deactivate first-person rig
            xrRigs.birdEyeRig.SetActive(true);      // Activate bird's-eye rig
        }
    }
}

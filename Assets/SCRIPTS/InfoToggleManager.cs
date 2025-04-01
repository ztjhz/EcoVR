using UnityEngine;

public class InfoToggleManager : MonoBehaviour
{
    public bool isInfoActive = false;

    public void ToggleDetectionRadius()
    {
        isInfoActive = !isInfoActive;
    }
}

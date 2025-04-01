using UnityEngine;

public class ToggleStatusBar : MonoBehaviour
{
    private float radius;
    private InfoToggleManager infoToggleManager;
    private bool isStatusActive;

    private void Start()
    {
        infoToggleManager = GameObject.Find("InfoToggleManager").GetComponent<InfoToggleManager>();
        ToggleStatus();
    }

    private void Update()
    {
        if (isStatusActive != infoToggleManager.isInfoActive)
        {
            isStatusActive = infoToggleManager.isInfoActive;
            ToggleStatus();
        }

    }

    private void ToggleStatus()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isStatusActive);
        }
    }
}

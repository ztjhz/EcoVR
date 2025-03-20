using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleItemsController : MonoBehaviour
{
    public GameObject binoculars;
    public GameObject magnifyingGlass;
    public InputActionReference toggleBinocularsAction;
    public InputActionReference toggleMagnifyingGlassAction;

    private void Awake()
    {
        toggleBinocularsAction.action.Enable();
        toggleMagnifyingGlassAction.action.Enable();

        toggleBinocularsAction.action.performed += ToggleBinoculars;
        toggleMagnifyingGlassAction.action.performed += ToggleMagnifyingGlass;

        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void ToggleBinoculars(InputAction.CallbackContext context)
    {
        binoculars.SetActive(!binoculars.activeSelf);
    }

    private void ToggleMagnifyingGlass(InputAction.CallbackContext context)
    {
        magnifyingGlass.SetActive(!magnifyingGlass.activeSelf);
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch(change)
        {
            case InputDeviceChange.Disconnected:
                toggleBinocularsAction.action.Disable();
                toggleMagnifyingGlassAction.action.Disable();

                toggleBinocularsAction.action.performed -= ToggleBinoculars;
                toggleMagnifyingGlassAction.action.performed -= ToggleMagnifyingGlass;
                break;
            case InputDeviceChange.Reconnected:
                toggleBinocularsAction.action.Enable();
                toggleMagnifyingGlassAction.action.Enable();

                toggleBinocularsAction.action.performed += ToggleBinoculars;
                toggleMagnifyingGlassAction.action.performed += ToggleMagnifyingGlass;
                break;
        }
    }
}
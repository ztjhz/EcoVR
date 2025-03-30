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

    public GameObject food;
    public GameObject drink;
    public InputActionReference toggleFoodAction;
    public InputActionReference toggleDrinkAction;

    private void Awake()
    {
        toggleBinocularsAction.action.Enable();
        toggleMagnifyingGlassAction.action.Enable();
        toggleFoodAction.action.Enable();
        toggleDrinkAction.action.Enable();

        toggleBinocularsAction.action.performed += ToggleBinoculars;
        toggleMagnifyingGlassAction.action.performed += ToggleMagnifyingGlass;
        toggleFoodAction.action.performed += ToggleFood;
        toggleDrinkAction.action.performed += ToggleDrink;

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
    private void ToggleFood(InputAction.CallbackContext context)
    {
        food.SetActive(!food.activeSelf);
    }

    private void ToggleDrink(InputAction.CallbackContext context)
    {
        drink.SetActive(!drink.activeSelf);
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch(change)
        {
            case InputDeviceChange.Disconnected:
                toggleBinocularsAction.action.Disable();
                toggleMagnifyingGlassAction.action.Disable();
                toggleFoodAction.action.Disable();
                toggleDrinkAction.action.Disable();

                toggleBinocularsAction.action.performed -= ToggleBinoculars;
                toggleMagnifyingGlassAction.action.performed -= ToggleMagnifyingGlass;
                toggleFoodAction.action.performed -= ToggleFood;
                toggleDrinkAction.action.performed -= ToggleDrink;
                break;
            case InputDeviceChange.Reconnected:
                toggleBinocularsAction.action.Enable();
                toggleMagnifyingGlassAction.action.Enable();
                toggleFoodAction.action.Enable();
                toggleDrinkAction.action.Enable();

                toggleBinocularsAction.action.performed += ToggleBinoculars;
                toggleMagnifyingGlassAction.action.performed += ToggleMagnifyingGlass;
                toggleFoodAction.action.performed += ToggleFood;
                toggleDrinkAction.action.performed += ToggleDrink;
                break;
        }
    }
}
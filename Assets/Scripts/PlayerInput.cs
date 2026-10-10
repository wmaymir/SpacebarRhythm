using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    //Unity's new input system has less latenency
    private InputSystem_Actions _inputActions;

    public static event Action<string> OnKeyPressed;

    void Awake()
    {
        _inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _inputActions.Player.OnlyChoice.performed += OnOnlyPressOption;
        //necessary because the Input System keeps action maps disabled by default to not waste resources
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        //unsubscribing prevents memory leaks
        _inputActions.Player.OnlyChoice.performed -= OnOnlyPressOption;
        _inputActions.Player.Disable();
    }

    public void OnOnlyPressOption(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnKeyPressed?.Invoke("Cb-A"); //stands for combination A
        }
    }
}

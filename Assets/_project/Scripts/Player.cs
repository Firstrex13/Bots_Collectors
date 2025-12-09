using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Input _playerInput;

    public event Action LMBPressed;
    public Input PlayerInput => _playerInput;

    private void Awake()
    {
        _playerInput = new Input();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.Actions.performed += OnMouseClickPerformed;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Player.Actions.performed -= OnMouseClickPerformed;
    }

    private void OnMouseClickPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LMBPressed?.Invoke();
        }
    }
}

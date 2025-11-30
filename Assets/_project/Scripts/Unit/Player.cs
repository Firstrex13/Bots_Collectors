using UnityEngine;

public class Player : MonoBehaviour
{
    private Input _playerInput;

    public Input PlayerInput => _playerInput;

    private void Awake()
    {
        _playerInput = new Input();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }
}

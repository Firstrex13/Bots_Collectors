using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private Transform _transform;
    private int _screenBorder = 100;
    private float _screenWidth;
    private float _screenHeigth;

    private void Start()
    {
        _transform = transform;
        _screenWidth = Screen.width;
        _screenHeigth = Screen.height;
    }

    private void LateUpdate()
    {
        MoveByMouse();
    }

    private void MoveByMouse()
    {
        float speed = _moveSpeed * Time.deltaTime;

        if (Mouse.current.position.ReadValue().x > _screenWidth - _screenBorder && Mouse.current.position.ReadValue().x < _screenWidth)
        {
            _transform.position += Vector3.right * speed;
        }
        else if (Mouse.current.position.ReadValue().x > (_screenWidth - Screen.width) && Mouse.current.position.ReadValue().x < _screenWidth - (Screen.width - _screenBorder))
        {
            _transform.position += -Vector3.right * speed;
        }
        else if (Mouse.current.position.ReadValue().y > _screenHeigth - 100 && Mouse.current.position.ReadValue().y < _screenHeigth)
        {
            _transform.position += Vector3.forward * speed;
        }
        else if (Mouse.current.position.ReadValue().y > (_screenHeigth - Screen.height) && Mouse.current.position.ReadValue().y < _screenHeigth - (Screen.height - _screenBorder))
        {
            _transform.position += -Vector3.forward * speed;
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Camera))]

public class CameraMove : MonoBehaviour
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

    private void Update()
    {
        MoveByMouse();
    }

    private void MoveByMouse()
    {
        float speed = _moveSpeed * Time.deltaTime;

        if (Input.mousePosition.x > _screenWidth - _screenBorder && Input.mousePosition.x < _screenWidth)
        {
            _transform.position += Vector3.right * speed;
        }
        else if (Input.mousePosition.x > (_screenWidth - Screen.width) && Input.mousePosition.x < _screenWidth - (Screen.width - 100))
        {
            _transform.position += -Vector3.right * speed;
        }
        else if (Input.mousePosition.y > _screenHeigth - 100 && Input.mousePosition.y < _screenHeigth)
        {
            _transform.position += Vector3.forward * speed;
        }
        else if (Input.mousePosition.y > (_screenHeigth - Screen.height) && Input.mousePosition.y < _screenHeigth - (Screen.height - 100))
        {
            _transform.position += -Vector3.forward * speed;
        }
    }
}

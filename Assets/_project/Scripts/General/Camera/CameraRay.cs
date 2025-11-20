using UnityEngine;

public class CameraRay : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rayDistance;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        SetRay();
    }

    private void OnValidate()
    {
        _camera ??= GetComponent<Camera>();
    }

    private void SetRay()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit hitInfo, _rayDistance);
    }
}

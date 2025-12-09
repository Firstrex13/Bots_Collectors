using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRay : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rayDistance;
    [SerializeField] private SelectableObject _selectableObject;

    [SerializeField] private Vector3 _groundPoint;

    public Vector3 GroundPoint => _groundPoint;

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
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        Physics.Raycast(ray, out RaycastHit hitInfo, _rayDistance);

        if (hitInfo.collider == null)
        {
            return;
        }

        if (hitInfo.collider.TryGetComponent(out SelectableObject selectable))
        {
            _selectableObject = selectable;
            _selectableObject.MakeHovered();
        }
        else
        {
            if (_selectableObject != null)
            {
                _selectableObject.MakeUnhovered();
                _selectableObject = null;
            }
        }

        if (hitInfo.collider.TryGetComponent<Ground>(out _))
        {
            _groundPoint = hitInfo.point;
        }
               
    }
}

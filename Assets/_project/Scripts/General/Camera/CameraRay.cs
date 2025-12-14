using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRay : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rayDistance;
    [SerializeField] private Selector _selector;

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

        if (hitInfo.collider.TryGetComponent<Base>(out Base baseItem))
        {
            _selector.MakeHovered(baseItem);
        }
        else
        {
            if (_selector != null)
            {
                _selector.MakeUnhovered();

            }
        }

        if (hitInfo.collider.TryGetComponent<Ground>(out _))
        {
            _groundPoint = hitInfo.point;
        }         
    }
}

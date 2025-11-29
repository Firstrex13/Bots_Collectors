using System;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    [SerializeField] private float _takeDistance;
    [SerializeField] private float _holdDistance;

    [SerializeField] private PickingObject _currentObject;
    [SerializeField] private PickingObject _aimedObject;

    private const float distanceToPickUp = 1f;

    public PickingObject CurrentObject => _currentObject;

    public event Action<Vector3> GotTarget;
    public event Action<ObjectPicker> GotObject;
    public event Action HaveNoCurrentObject;

    private void Update()
    {
        if (_aimedObject != null)
        {
            float distanceToTarget;

            distanceToTarget = Vector3.Distance(_aimedObject.transform.position, transform.position);

            if (distanceToTarget <= distanceToPickUp)
            {
                PickUp();
            }
        }

        if(_currentObject != null)
        {
            GotObject?.Invoke(this);
        }
    }

    public void PickUp()
    {
        if (_currentObject != null)
        {
            return;
        }

        if (_aimedObject.TryGetComponent(out PickingObject pickingObject) == false)
            return;

        _currentObject = pickingObject;
        _currentObject.PickUp(transform, _holdDistance);
    }

    public void Drop()
    {
        if (_currentObject != null)
        {
            _currentObject.Drop();
            _currentObject = null;
            HaveNoCurrentObject?.Invoke();

        }
    }

    public void SetAime(PickingObject pickingObject)
    {
        _aimedObject = null;
        _aimedObject = pickingObject;
        GotTarget?.Invoke(_aimedObject.transform.position);
    }
}



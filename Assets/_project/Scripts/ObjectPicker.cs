using System;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    [SerializeField] private float _takeDistance;
    [SerializeField] private float _holdDistance;

    [SerializeField] private PickingObject _currentObject;
    [SerializeField] private PickingObject _aimedObject;

    public event Action HaveNoCurrentObject;

    public PickingObject CurrentObject => _currentObject;

    public void PickUp()
    {
        if (_currentObject != null)
        {
            return;
        }

        if (_aimedObject != null)
        {
            Debug.Log("Picked");
            _currentObject = _aimedObject;
            _currentObject.PickUp(transform, _holdDistance);
        }
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

    public void SetAimedObject(PickingObject pickingObject)
    {
        _aimedObject = pickingObject;
    }
}



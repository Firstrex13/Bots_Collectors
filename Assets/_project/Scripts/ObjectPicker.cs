using System;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    [SerializeField] private float _takeDistance;
    [SerializeField] private float _holdDistance;

    [SerializeField] private PickingObject _currentObject;

    public PickingObject CurrentObject => _currentObject;

    public void PickUp(Collider collider)
    {
        if (collider.TryGetComponent(out PickingObject pickingObject) == false)
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
        }
    }
}



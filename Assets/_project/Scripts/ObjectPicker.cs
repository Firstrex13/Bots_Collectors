using System;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    [SerializeField] private float _takeDistance;
    [SerializeField] private float _holdDistance;

    [SerializeField] private PickingObject _currentObject;

    public event Action BecameFree;

    private void OnDisable()
    {
        if (_currentObject == null)
            return;

        if (_currentObject != null)
            _currentObject.Dropped -= NotifyAboutStatus;
    }

    public void PickUp(Collider collider)
    {


        if (collider.TryGetComponent(out PickingObject pickingObject) == false)
            return;

        _currentObject = pickingObject;
        _currentObject.PickUp(transform, _holdDistance);
    }

    private void NotifyAboutStatus()
    {
        BecameFree?.Invoke();
        Debug.Log("пикер освободился");
    }

    public void Drop()
    {
        if (_currentObject != null)
        {
            _currentObject.Drop();
            _currentObject.Dropped += NotifyAboutStatus;
            _currentObject = null;
            Debug.Log("метод дроп");
        }
    }
}



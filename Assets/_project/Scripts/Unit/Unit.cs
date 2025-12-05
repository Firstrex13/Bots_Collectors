using System;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Rigidbody))]

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private ObjectPicker _picker;
    [SerializeField] private UnitMover _mover;
    [SerializeField] private Transform _basePosition;
    [SerializeField] private Transform _watingZone;

    private bool _pickedUp;

    public event Action<Unit> BecameFree;

    private void OnEnable()
    {
        _mover.ReachedTarget += OnReachedTarget;
        _picker.HaveNoCurrentObject += GoToWaitingZone;
    }

    private void OnDisable()
    {
        _mover.ReachedTarget -= OnReachedTarget;
        _picker.HaveNoCurrentObject -= GoToWaitingZone;
    }

    public void DropObject()
    {
        _picker.Drop();
    }

    public void SendForResourse(PickingObject pickingObject)
    {
        _pickedUp = false;
        _picker.SetAimedObject(pickingObject);
        _mover.GoToTarget(pickingObject.transform.position);
    }

    private void GoToWaitingZone()
    {
        _mover.GoToTarget(_watingZone.position);
    }

    private void OnReachedTarget()
    {
        if (_pickedUp)
        {
            _picker.Drop();
            BecameFree?.Invoke(this);
        }
        else
        {
            _picker.PickUp();
            _mover.GoToTarget(_basePosition.position);
            _pickedUp = true;
        }
    }
}


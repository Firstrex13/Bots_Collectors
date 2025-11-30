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

    public event Action<Unit> BecameFree;

    private void OnEnable()
    {
        _picker.GotTarget += _mover.GoToTarget;
        _picker.HaveNoCurrentObject += SendMessageBecameFreeEvent;

    }

    private void OnDisable()
    {
        _picker.GotTarget -= _mover.GoToTarget;
        _picker.HaveNoCurrentObject -= SendMessageBecameFreeEvent;

    }

    public void DropObject()
    {
        _picker.Drop();
    }

    public void SendMessageBecameFreeEvent()
    {
        BecameFree?.Invoke(this);
    }
}


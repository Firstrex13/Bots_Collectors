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

    private bool _pickedUp;

    public event Action DroppedObject;
    public event Action<Unit> BecameFree;

    private void OnEnable()
    {
      //  _mover.ReachedTarget += OnReachedTarget;


        //  _mover.AimeChanged += _mover.GoToTarget;
        //   _mover.ReachedStorage += MessageCameToStorage;
        _picker.HaveNoCurrentObject += SendMessageBecameFreeEvent;

    }

    private void OnDisable()
    {
      //  _mover.ReachedTarget -= OnReachedTarget;

        //  _mover.AimeChanged -= _mover.GoToTarget;
        //   _mover.ReachedStorage -= MessageCameToStorage;
        _picker.HaveNoCurrentObject -= SendMessageBecameFreeEvent;
    }

    public void DropObject()
    {
        _picker.Drop();
    }

    public void MessageCameToStorage()
    {
        DropObject();
        DroppedObject?.Invoke();
    }

    public void SendMessageBecameFreeEvent()
    {
        BecameFree?.Invoke(this);
    }

    public void SendForResourse(PickingObject pickingObject)
    {
        _pickedUp = false;
        _picker.SetAimedObject(pickingObject);
        _mover.GoToTarget(pickingObject.transform.position);
    }

    private void OnReachedTarget()
    {
        Debug.Log("Action");
        //if (_pickedUp)
        //{
        //    _picker.Drop();
        //}
        //else
        //{
        //    _picker.PickUp();
        //    _mover.GoToTarget(_basePosition.position);
        //}
    }
}


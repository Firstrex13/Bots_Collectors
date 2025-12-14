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
    [SerializeField] private BaseBuilder _baseBuilder;

    [SerializeField] private Transform _basePosition;
    [SerializeField] private Transform _watingZone;

    public event Action<Unit> BecameFree;

    private void OnEnable()
    {
        _picker.HaveNoCurrentObject += GoToWaitingZone;
    }

    private void OnDisable()
    {
        _picker.HaveNoCurrentObject -= GoToWaitingZone;
    }

    public void Initialize(Transform basePosition, Transform waitingZone)
    {
        _basePosition = basePosition;
        _watingZone = waitingZone;
        
    }

    public void Initialize(BaseBuilder baseBuilder)
    {
        _baseBuilder = baseBuilder;
    }

    public void DropObject()
    {
        _picker.Drop();
    }

    public void SendForResourse(PickingObject pickingObject)
    {
        _picker.SetAimedObject(pickingObject);
        _mover.GoToTarget(pickingObject.transform.position, GoToBase);
    }

    public void BuildBase(Vector3 position, Unit unit)
    {
        _baseBuilder.BuildNewBase(position, this);
    }

    private void GoToBase()
    {
        _picker.PickUp();
        _mover.GoToTarget(_basePosition.position, GiveResourseToBase);
    }

    private void GiveResourseToBase()
    {
        _picker.Drop();
        BecameFree?.Invoke(this);
    }

    public void GoToTarget(Vector3 position, Action action)
    {
        _mover.GoToTarget(position, action);
    }

    private void GoToWaitingZone()
    {
        _mover.GoToTarget(_watingZone.position, null);
    }
}


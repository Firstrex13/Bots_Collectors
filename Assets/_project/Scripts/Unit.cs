using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
[RequireComponent(typeof(Rigidbody))]

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private ObjectPicker _picker;

    [SerializeField] private bool _isFree;

    [SerializeField] private bool _isHolding; 

    private NavMeshAgent _agent;
    private Transform _transform;

    public event Action<Unit> ReadyGoToStorage;
    public event Action<Unit> BecameFree;

    public bool IsFree => _isFree;

    private void OnEnable()
    {
        _picker.BecameFree += MakeFree;
    }

    private void OnDisable()
    {
        _picker.BecameFree -= MakeFree;
    }

    private void Start()
    {
        _isHolding = false;
        _agent = GetComponent<NavMeshAgent>();
        _transform = transform;
        MakeStepForward();
    }

    public void Initialize()
    {
        _isFree = true;
    }

    public void GoToResourse(Vector3 position)
    {
        _agent.SetDestination(position);
    }

    public void GoToStorage(Vector3 position)
    {
        _agent.SetDestination(position);
    }

    public void GoToWaitngZone(Vector3 position)
    {
        _agent.SetDestination(position);
    }

    public void MakeUnitOcupied()
    {
        _isFree = false;
    }

    private void MakeFree()
    {
        _isFree = true;
    }

    private void MakeStepForward()
    {
        _transform.DOMove(Vector3.one, _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PickingObject>(out PickingObject pickingObject))
        {
            if (pickingObject.TryGetComponent<Resourse>(out Resourse resourse))
            {
                if (resourse.Collected && _isHolding == false)
                {
                    _picker.PickUp(other);
                    ReadyGoToStorage?.Invoke(this);
                    _isHolding = true;
                }
            }
        }

        if (other.TryGetComponent<Storage>(out _) && _isHolding)
        {
            _picker.Drop();
            BecameFree?.Invoke(this);
            _isHolding = false;
        }
    }
}

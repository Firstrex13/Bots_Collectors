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
    public bool IsHolding => _isHolding;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

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
        _isFree = true;
        _isHolding = false;

        _transform = transform;
    }

    public void Initialize()
    {
        _isFree = true;
    }

    public void GoToTarget(Vector3 position)
    {
        _agent.SetDestination(position);
    }

    public void MakeUnitOcupied()
    {
        _isFree = false;
    }

    public void MakeIsHolding()
    {
        _isHolding = true;
    }

    public void MakeNotIsHolding()
    {
        _isHolding = false;
    }

    public void DropObject()
    {
        _picker.Drop();
    }

    public void PickUpObject(Collider other)
    {
        _picker.PickUp(other);
    }

    public void SendReadyToGoStorageEvent()
    {
        ReadyGoToStorage?.Invoke(this);
    }

    public void SendMessageBecameFreeEvent()
    {
        BecameFree?.Invoke(this);
    }

    private void MakeFree()
    {
        _isFree = true;
    }
}


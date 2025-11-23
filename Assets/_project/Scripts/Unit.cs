using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
[RequireComponent(typeof(Rigidbody))]

public class Unit : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private ObjectPicker _picker;

    [SerializeField] private bool _isFree;

    private NavMeshAgent _agent;
    private Transform _transform;

    public event Action<Unit> ReadyGoToBase;

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
        _agent = GetComponent<NavMeshAgent>();
        _rigidBody = GetComponent<Rigidbody>();
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

    public void GoToBase(Vector3 position)
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
        Debug.Log("юнит освободился");
    }

    private void MakeStepForward()
    {
        _transform.DOMove( Vector3.one, _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PickingObject>(out _))
        {
            _picker.PickUp(other);
            ReadyGoToBase?.Invoke(this);
        } 

        if(other.TryGetComponent<Storage>(out _))
        {
            Debug.Log("зашел в тригер");
            _picker.Drop();           
        }
    }
}

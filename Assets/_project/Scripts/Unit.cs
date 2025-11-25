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

    private NavMeshAgent _agent;
    private Transform _transform;

    public event Action<Unit> ReadyGoToStorage;
    public event Action<Unit> BecameFree;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void GoToTarget(Vector3 position)
    {
        _agent.SetDestination(position);
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
}


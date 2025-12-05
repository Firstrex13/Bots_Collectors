using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class UnitMover : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform _transform;
    private float _distanceToInteracte = 2f;

    private Coroutine _move;

    public event Action ReachedTarget;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _transform = transform;
    }

    private void OnDisable()
    {
        if (_move != null)
        {
            StopCoroutine(_move);
        }
    }

    public void GoToTarget(Vector3 position)
    {
        if (_move != null)
        {
            StopCoroutine(_move);
        }

        _move = StartCoroutine(Move(position));

    }

    private IEnumerator Move(Vector3 position)
    {
        _agent.SetDestination(position);

        while (_agent.remainingDistance > _distanceToInteracte)
        {
            yield return null;
        }

        ReachedTarget?.Invoke();
    }
}

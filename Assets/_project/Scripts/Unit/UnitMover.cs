using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class UnitMover : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform _transform;
    private float _distanceToInteracte = 2f;

    private Coroutine _checkDistance;

    public event Action ReachedTarget;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _transform = transform;
    }

    private void OnDisable()
    {
        if (_checkDistance != null)
        {
            StopCoroutine(_checkDistance);
        }
    }

    public void GoToTarget(Vector3 position)
    {
        _agent.SetDestination(position);

        if (_checkDistance != null)
        {
            StopCoroutine(_checkDistance);
        }

        _checkDistance = StartCoroutine(CheckDistance(position));
    }

    private IEnumerator CheckDistance(Vector3 position)
    {
        yield return null;

        while (_agent.remainingDistance > _distanceToInteracte)
        {
            yield return null;
        }

        _checkDistance = null;
        ReachedTarget?.Invoke();
    }
}

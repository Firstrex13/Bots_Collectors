using UnityEngine;
using UnityEngine.AI;

public class UnitMover : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform _transform;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void GoToTarget(Vector3 position)
    {
        _agent.SetDestination(position);
    }
}

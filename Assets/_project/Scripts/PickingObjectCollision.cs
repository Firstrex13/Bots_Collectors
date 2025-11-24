using UnityEngine;

[RequireComponent(typeof(Resourse))]

public class PickingObjectCollision : MonoBehaviour
{
    [SerializeField] private Resourse _resourse;

    private void OnValidate()
    {
        _resourse ??= GetComponent<Resourse>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Storage>(out Storage storage))
        {
            _resourse.SendMessageBroughtOnBaseEvent();
            storage.IncreaseCount();
        }
    }
}

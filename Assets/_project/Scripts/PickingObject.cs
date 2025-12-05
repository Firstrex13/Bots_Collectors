using System;
using UnityEngine;

public class PickingObject : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody _rigidbody;

    public event Action<PickingObject> ReadyToBackToPull;
    public event Action Dropped;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
    }

    public void Initialize(Vector3 position)
    {
        _transform.position = position;
    }

    public void PickUp(Transform parent, float holdDistance)
    {
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0, 0, holdDistance);

        _rigidbody.isKinematic = true;
    }

    public void Drop()
    {
        transform.SetParent(null);
        _rigidbody.isKinematic = false;
        ReadyToBackToPull?.Invoke(this);
        Dropped?.Invoke();
    }
}

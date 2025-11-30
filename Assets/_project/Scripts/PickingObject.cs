using System;
using UnityEngine;

public class PickingObject : MonoBehaviour
{
    private const float _zone = 20;
    private const float _height = 0.5f;

    private Transform _transform;
    private Rigidbody _rigidbody;

    public event Action<PickingObject> Dropped;

    private void Awake()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        _transform.position = new Vector3(UnityEngine.Random.Range(-_zone, _zone), _height, UnityEngine.Random.Range(-_zone, _zone));
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
        Dropped?.Invoke(this);
    }
}

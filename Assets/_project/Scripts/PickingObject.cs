using System;
using UnityEngine;

public class PickingObject : MonoBehaviour
{
    [SerializeField] private bool _aimed = false;
    [SerializeField] private bool _scanned = false;
    [SerializeField] private bool _collected = false;

    private Rigidbody _rigidbody;

    public bool Scanned => _scanned;
    public bool Aimed => _aimed;
    public bool Collected => _collected;

    public event Action Dropped;

    private void OnEnable()
    {
        _collected = false;
        _scanned = false;
        _aimed = false;
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
        Dropped?.Invoke();
    }

    public void MakeObjectScanned()
    {
        _scanned = true;
    }
    public void MakeObjectAimed()
    {
        _aimed = true;
    }

    public void MakeCollected()
    {
        _collected = true;
    }
}

using System;
using UnityEngine;

[SelectionBase]

public class Resourse : MonoBehaviour
{
    [SerializeField] private bool _collected = false;
    [SerializeField] private bool _scanned = false;

    private Transform _transform;

    public event Action<Resourse> BroughtOnBase;
    public bool Scanned => _scanned;
    public bool Collected => _collected;

    private void Awake()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        _scanned = false;
        _collected = false;
        _transform.position = new Vector3(UnityEngine.Random.Range(-20, 20), 0.5f, UnityEngine.Random.Range(-20, 20));
    }

    public void MakeObjectScanned()
    {
        _scanned = true;
    }

    public void MakeObjectCollected()
    {
        _collected = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Storage>(out _))
        {
            BroughtOnBase?.Invoke(this);
        }
    }
}

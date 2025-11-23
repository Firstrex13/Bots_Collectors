using System;
using UnityEngine;

[SelectionBase]

public class Resourse : MonoBehaviour
{
    [SerializeField] private bool _aimed = false;
    [SerializeField] private bool _scanned = false;
    [SerializeField] private bool _collected = false;

    private const float _zone = 20;
    private const float _height = 0.5f;

    private Transform _transform;

    public event Action<Resourse> BroughtOnBase;
    public bool Scanned => _scanned;
    public bool Aimed => _aimed;
    public bool Collected => _collected;

    private void Awake()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        _collected = false;
        _scanned = false;
        _aimed = false;
        _transform.position = new Vector3(UnityEngine.Random.Range(-_zone, _zone), _height, UnityEngine.Random.Range(-_zone, _zone));
    }

    public void MakeObjectScanned()
    {
        _scanned = true;
    }

    public void MakeObjectAimed()
    {
        _aimed = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Storage>(out Storage storage))
        {
            BroughtOnBase?.Invoke(this);
            storage.IncreaseCount();
        }

        if(other.TryGetComponent<Unit>(out _))
        {
            _collected |= true;
        }
    }
}

using System;
using UnityEngine;

[SelectionBase]

public class Resourse : PickingObject
{
    private const float _zone = 20;
    private const float _height = 0.5f;

    private Transform _transform;

    public event Action<Resourse> BroughtOnBase;

    private void Awake()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        _transform.position = new Vector3(UnityEngine.Random.Range(-_zone, _zone), _height, UnityEngine.Random.Range(-_zone, _zone));
    }

    public void SendMessageBroughtOnBaseEvent()
    {
        BroughtOnBase?.Invoke(this);
    }
}

using System;
using System.Collections;
using UnityEngine;

public class ResoursesSpawner : MonoBehaviour
{
    private const float _zone = 20;
    private const float _height = 0.5f;

    [SerializeField] private PickingObject _prefab;
    [SerializeField] private int _count;

    private ObjectPool<PickingObject> _pool;

    private float _delay = 3f;

    private Coroutine _createCoroutine;

    public event Action Created;

    private void Start()
    {
        _pool = new ObjectPool<PickingObject>(_prefab, _count);

        if (_createCoroutine != null)
        {
            StopCoroutine(_createCoroutine);
        }

        _createCoroutine = StartCoroutine(Create());
    }

    private void OnDestroy()
    {
        StopCoroutine(_createCoroutine);
    }

    private void OnReturnToPool(PickingObject resourse)
    {
        _pool.ReturnObject(resourse);
        resourse.ReadyToBackToPull -= OnReturnToPool;
    }

    private IEnumerator Create()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        while (enabled)
        {
            yield return delay;
            PickingObject resourse = _pool.GetFromPool();
            Created?.Invoke();
            resourse.ReadyToBackToPull += OnReturnToPool;
            resourse.Initialize(new Vector3(UnityEngine.Random.Range(-_zone, _zone), _height, UnityEngine.Random.Range(-_zone, _zone)));
        }
    } 
}

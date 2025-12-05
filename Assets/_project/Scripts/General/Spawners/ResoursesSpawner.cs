using System;
using System.Collections;
using UnityEngine;

public class ResoursesSpawner : MonoBehaviour
{
    [SerializeField] private PickingObject _prefab;
    [SerializeField] private int _count;

    private ObjectPool<PickingObject> _pool;

    private float _delay = 3f;

    private Coroutine _createCoroutine;

    public event Action Created;
    public event Action<PickingObject> Returned;

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
        resourse.Dropped -= OnReturnToPool;
    }

    private IEnumerator Create()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        while (enabled)
        {
            yield return delay;
            PickingObject resourse = _pool.GetFromPool();
            Created?.Invoke();
            resourse.Dropped += OnReturnToPool;
            Returned?.Invoke(resourse);
        }
    }
}

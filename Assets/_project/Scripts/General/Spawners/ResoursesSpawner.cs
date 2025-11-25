using System;
using System.Collections;
using UnityEngine;

public class ResoursesSpawner : MonoBehaviour
{
    [SerializeField] private Resourse _prefab;
    [SerializeField] private int _count;

    private ObjectPool<Resourse> _pool;

    private float _delay = 3f;

    private Coroutine _createCoroutine;

    public event Action Created;
    public event Action<Resourse> Returned;

    private void Start()
    {
        _pool = new ObjectPool<Resourse>(_prefab, _count);
        _createCoroutine = StartCoroutine(Create());
    }

    private void OnDestroy()
    {
        StopCoroutine(_createCoroutine);
    }

    private void OnReturnToPool(Resourse resourse)
    {
        _pool.ReturnObject(resourse);
        resourse.BroughtOnBase -= OnReturnToPool;
    }

    private IEnumerator Create()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        while (enabled)
        {
            yield return delay;
            Resourse resourse = _pool.GetFromPool();
            Created?.Invoke();
            resourse.BroughtOnBase += OnReturnToPool;
            Returned?.Invoke(resourse);
        }
    }
}

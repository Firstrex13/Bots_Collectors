using System.Collections;
using UnityEngine;

public class ResoursesSpawner : MonoBehaviour
{
    [SerializeField] private Resourse _prefab;
    [SerializeField] private int _count;

    private ObjectPool<Resourse> _pool;
    private float _delay = 3f;

    private Coroutine _createCoroutine;

    private void Start()
    {
        _pool = new ObjectPool<Resourse>(_prefab, _count);
        _createCoroutine = StartCoroutine(nameof(Create));
    }

    private void Update()
    {

    }
    private void OnDestroy()
    {
        StopCoroutine(nameof(_createCoroutine));
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
            resourse.BroughtOnBase += OnReturnToPool;
        }
    }
}

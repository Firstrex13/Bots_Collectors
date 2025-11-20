using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private Queue<T> _pool = new Queue<T>();

    private T _prefab;

    public Action Created;

    public ObjectPool(T prefab, int count)
    {
        _prefab = prefab;

        for (int i = 0; i < count; i++)
        {
            _pool.Enqueue(FillUpPool());
        }
    }

    public T GetFromPool()
    {
        if (_pool.Count > 0)
        {
            T obj = _pool.Dequeue();
            obj.gameObject.SetActive(true);
            Created?.Invoke();
            return obj;
        }
        else
        {
            return UnityEngine.Object.Instantiate(_prefab);
        }
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);

        _pool.Enqueue(obj);
    }

    private T FillUpPool()
    {
        T obj = UnityEngine.Object.Instantiate(_prefab);
        obj.gameObject.SetActive(false);

        return obj;
    }
}

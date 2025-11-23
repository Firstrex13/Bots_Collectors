using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private int _resoursesCount;

    public event Action<int> Updated;
    public int Count => _resoursesCount;

    public void IncreaseCount()
    {
        _resoursesCount++;
        Updated?.Invoke(Count);
    }
}

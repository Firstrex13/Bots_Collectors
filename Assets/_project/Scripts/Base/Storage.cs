using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private int _resoursesCount;
    [SerializeField] private Base _base;

    public event Action<int> Updated;
    public event Action IsEnoughForUnit;
    public event Action IsEnoughForBase;
    public int ResourseCount => _resoursesCount;

    public void IncreaseCount()
    {
        _resoursesCount++;
        Updated?.Invoke(ResourseCount);

        if (_base.CurrentState == Base.State.BuildingUnits && _resoursesCount >= _base.UnitCost)
        {
            IsEnoughForUnit?.Invoke();
        }

        if(_base.CurrentState == Base.State.BuildingNewBase && _resoursesCount >= _base.BaseCost)
        {
            IsEnoughForBase?.Invoke();
        }
    }

    public void SpendResourse(int cost)
    {
        if (cost > 0)
        {
            _resoursesCount -= cost;
        }

        if(_resoursesCount < 0)
        {
            _resoursesCount = 0;
        }

        Updated?.Invoke(_resoursesCount);
    }
}

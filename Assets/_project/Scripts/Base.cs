using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private List<Unit> _units = new List<Unit>();

    private int _startCount = 3;

    private void Start()
    {
        for (int i = 0; i < _startCount; i++)
        {
            _unitSpawner.Create(_spawnPoint);
        }
    }
}

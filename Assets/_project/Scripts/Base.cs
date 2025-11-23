using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Radar _radar;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private Player _player;
    [SerializeField] private Storage _storage;

    private float _delay = 2f;
    private int _startCount = 3;

    private void OnEnable()
    {
        _radar.ResourseFound += SendForResourse;
    }

    private void OnDisable()
    {
        _radar.ResourseFound -= SendForResourse;
    }

    private void Start()
    {
        _player.PlayerInput.Player.Actions.performed += ctx => _radar.OnScanArea();

        StartCoroutine(nameof(CreateUnits));
    }

  
    private IEnumerator CreateUnits()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        for (int i = 0; i < _startCount; i++)
        {
            Unit unit = _unitSpawner.Create(_spawnPoint);

            unit.Initialize();
            unit.ReadyGoToBase += SendUnitBack;

            _units.Add(unit);
            yield return delay;
        }
    }

    public void SendUnitBack(Unit unit)
    {
        unit.GoToBase(_storage.transform.position);
        unit.ReadyGoToBase -= SendUnitBack;
    }

    private void SendForResourse()
    {
        if (_radar.Resourses != null)
        {
            foreach (var item in _radar.Resourses)
            {
                foreach (var unit in _units)
                {
                    if (unit.IsFree && !item.Collected)
                    {
                        unit.GoToResourse(item.transform.position);
                        unit.MakeUnitOcupied();
                        item.MakeObjectCollected();
                    }
                }
            }
        }
    }
}

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
    [SerializeField] private Transform _watingZone;

    private float _delay = 2f;
    private int _startCount = 3;

    private Coroutine _createUnitsCoroutine;

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
       _createUnitsCoroutine = StartCoroutine(nameof(CreateUnits));
    }

    private void OnDestroy()
    {
        StopCoroutine(_createUnitsCoroutine);
    }

    private IEnumerator CreateUnits()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        for (int i = 0; i < _startCount; i++)
        {
            Unit unit = _unitSpawner.Create(_spawnPoint);

            unit.Initialize();

            _units.Add(unit);
            yield return delay;
        }
    }

    public void SendUnitBack(Unit unit)
    {
        unit.GoToStorage(_storage.transform.position);
        unit.ReadyGoToStorage -= SendUnitBack;
    }

    public void SendUnitToWaitingZone(Unit unit)
    {
        unit.GoToWaitngZone(_watingZone.transform.position);
        unit.BecameFree -= SendUnitToWaitingZone;
    }

    private void SendForResourse()
    {
        if (_radar.Resourses != null)
        {
            foreach (var item in _radar.Resourses)
            {
                foreach (var unit in _units)
                {
                    if (unit.IsFree && !item.Aimed)
                    {
                        unit.GoToResourse(item.transform.position);
                        unit.MakeUnitOcupied();
                        item.MakeObjectAimed();
                        unit.ReadyGoToStorage += SendUnitBack;
                        unit.BecameFree += SendUnitToWaitingZone;
                    }
                }
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private Storage _storage;
    [SerializeField] private Radar _radar;
    [SerializeField] private PickingObjectsService _pickingObjectsService;
    [SerializeField] private Transform _watingZone;
    [SerializeField] private FlagPlacer _flagPlacer;
    [SerializeField] private Flag _flag;

    private List<Unit> _ocupiedUnits = new List<Unit>();
    private List<Unit> _freeUnits = new List<Unit>();

    private float _delay = 3f;
    private int _startCount = 3;
    private int _unitCost = 3;
    private int _baseCost = 5;

    private Coroutine _createUnitsCoroutine;

    public event Action<Vector3, Unit> BaseBuildRequested;

    public int UnitCost => _unitCost;
    public Flag Flag => _flag;



    private void OnEnable()
    {
        _radar.ResoursesFound += _pickingObjectsService.AddToList;
        _pickingObjectsService.ListUpdated += OnResourseFound;
        _storage.IsEnoughForUnit += CreateUnit;

    }

    private void OnDisable()
    {
        _radar.ResoursesFound -= _pickingObjectsService.AddToList;
        _pickingObjectsService.ListUpdated -= OnResourseFound;
        _storage.IsEnoughForUnit -= CreateUnit;

    }

    private void Start()
    {
        _flagPlacer.FlagPlaced += SendUnitToBuildBase;

        if (_createUnitsCoroutine != null)
        {
            StopCoroutine(_createUnitsCoroutine);
        }

        if (_freeUnits.Count < _startCount)
            _createUnitsCoroutine = StartCoroutine(CreateStartUnits());
    }

    private void OnDestroy()
    {
        _flagPlacer.FlagPlaced -= SendUnitToBuildBase;

        if (_createUnitsCoroutine != null)
            StopCoroutine(_createUnitsCoroutine);
    }

    private void OnValidate()
    {
        _radar ??= GetComponent<Radar>();
    }

    public void Initialize(FlagPlacer flagPlacer)
    {
        _flagPlacer = flagPlacer;
    }

    public void AddUnit(Unit unit)
    {
        _freeUnits.Add(unit);
    }

    public void ClearList()
    {
        _freeUnits.Clear();
    }

    private void CreateUnit()
    {
        Unit unit = _unitSpawner.Create(_spawnPoint);
        unit.Initialize(transform, _watingZone);
        _freeUnits.Add(unit);
        _storage.SpendResourse(_unitCost);
    }

    private IEnumerator CreateStartUnits()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        for (int i = 0; i < _startCount; i++)
        {
            Unit unit = _unitSpawner.Create(_spawnPoint);
            unit.Initialize(transform, _watingZone);

            if (unit.TryGetComponent<UnitMover>(out UnitMover mover))
            {
                _freeUnits.Add(unit);

                yield return delay;
            }
        }
    }

    private void SendUnitToBuildBase(Vector3 target)
    {
        if (_freeUnits.Count == 0)
        {
            return;
        }

        Unit unit = _freeUnits[0];

        unit.GoToTarget(target, () =>
            {
                BaseBuildRequested?.Invoke(target, unit);
            }
            );

        _freeUnits.Remove(unit);
        return;
    }

    private void SendUnitForResourse(PickingObject pickingObject)
    {
        if (_freeUnits.Count == 0)
        {
            return;
        }
        pickingObject.ReadyToBackToPull += RemoveFromList;
        Unit unit = _freeUnits[0];
        unit.SendForResourse(pickingObject);
        unit.BecameFree += MoveUnitToFree;
        _pickingObjectsService.PutResourseInOcupiedList(pickingObject);
        MoveUnitToOcupied(unit);
        return;
    }

    private void RemoveFromList(PickingObject pickingObject)
    {
        _pickingObjectsService.RemoveFromList(pickingObject);
        pickingObject.ReadyToBackToPull -= _pickingObjectsService.RemoveFromList;
    }

    private void MoveUnitToFree(Unit unit)
    {
        unit.BecameFree -= MoveUnitToFree;
        _freeUnits.Add(unit);
        _ocupiedUnits.Remove(unit);
        _storage.IncreaseCount();
    }

    private void MoveUnitToOcupied(Unit unit)
    {
        _ocupiedUnits.Add(unit);
        _freeUnits.Remove(unit);
    }

    private void OnResourseFound()
    {
        PickingObject resourse;

        resourse = _pickingObjectsService.GetFreeObject();

        if (resourse)
        {
            SendUnitForResourse(resourse);
        }
    }
}

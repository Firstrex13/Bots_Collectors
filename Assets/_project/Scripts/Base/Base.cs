using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public enum State
    {
        BuildingUnits,
        BuildingNewBase
    }

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private Storage _storage;
    [SerializeField] private Radar _radar;
    [SerializeField] private PickingObjectsService _pickingObjectsService;
    [SerializeField] private Transform _watingZone;
    [SerializeField] private Flag _flag;
    [SerializeField] private List<Unit> _freeUnits = new List<Unit>();
    [SerializeField] private BaseStates _states;

    private List<Unit> _ocupiedUnits = new List<Unit>();

    private int _unitCost = 3;
    private int _baseCost = 5;

    private Coroutine _createUnitsCoroutine;
    private Coroutine _wait;

    public int UnitCost => _unitCost;
    public int BaseCost => _baseCost;
    public Flag Flag => _flag;
    public Transform WatingZone => _watingZone;

    private void OnEnable()
    {
        _radar.ResoursesFound += _pickingObjectsService.AddToList;
        _pickingObjectsService.ListUpdated += OnResourseFound;
        _storage.IsEnoughForUnit += CreateUnit;
        _storage.IsEnoughForBase += SendUnitToBuildBase;
    }

    private void OnDisable()
    {
        _radar.ResoursesFound -= _pickingObjectsService.AddToList;
        _pickingObjectsService.ListUpdated -= OnResourseFound;
        _storage.IsEnoughForUnit -= CreateUnit;
        _storage.IsEnoughForBase -= SendUnitToBuildBase;
    }

    private void OnDestroy()
    {
        if (_createUnitsCoroutine != null)
            StopCoroutine(_createUnitsCoroutine);
    }

    private void OnValidate()
    {
        _radar ??= GetComponent<Radar>();
    }

    public void Initialize(UnitSpawner spawner, PickingObjectsService pickingObjectsService)
    {

        _unitSpawner = spawner;
        _pickingObjectsService = pickingObjectsService;
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
        if (_states.CurrentState == BaseStates.State.BuildingUnits)
        {
            Unit unit = _unitSpawner.Create(_spawnPoint);
            unit.Initialize(transform, _watingZone);
            _freeUnits.Add(unit);
            _storage.SpendResourse(_unitCost);
        }
    }

    private void SendUnitToBuildBase()
    {
        if (_states.CurrentState != BaseStates.State.BuildingNewBase)
        {
           _states.ChangeState();
        }

        if (_freeUnits.Count == 0)
        {
            return;
        }

        _storage.SpendResourse(_baseCost);

        Unit unit = _freeUnits[0];

        MoveUnitToOcupied(unit);

        unit.GoToTarget(_flag.transform.position, () =>
            {
                unit.BuildBase(_flag.transform.position, unit);
                _freeUnits.Remove(unit);
                _flag.TurnOff();
                unit.GoToWaitingZone();
                MoveUnitToFree(unit);
                _states.ChangeState();
            }
            );

        return;
    }

    private void SendUnitForResourse(PickingObject pickingObject)
    {
        if (_freeUnits.Count == 0)
        {
            return;
        }

        if(pickingObject == null)
        {
            return;
        }

        for (int i = 0; i < _freeUnits.Count; i++)
        {
            if (!_freeUnits[i].IsOcupied)
            {
                pickingObject.ReadyToBackToPull += RemoveFromList;
                _pickingObjectsService.PutResourseInOcupiedList(pickingObject);
                Unit unit = _freeUnits[i];
                unit.SendForResourse(pickingObject);
                unit.BecameFree += MoveUnitToFree;
                MoveUnitToOcupied(unit);
                return;
            }
        }
    }

    private void RemoveFromList(PickingObject pickingObject)
    {
        _pickingObjectsService.RemoveFromList(pickingObject);
        pickingObject.ReadyToBackToPull -= RemoveFromList;
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
        PickingObject resourses;

        resourses = _pickingObjectsService.GetFreeObjects();

        {
            SendUnitForResourse(resourses);
        }
    }
}

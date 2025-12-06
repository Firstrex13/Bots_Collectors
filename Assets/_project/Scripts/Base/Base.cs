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
    [SerializeField] private ResoursesSpawner _resourseSpawner;
    [SerializeField] private Transform _watingZone;

    private List<Unit> _ocupiedUnits = new List<Unit>();
    private List<Unit> _freeUnits = new List<Unit>();

    private float _delay = 3f;
    private int _startCount = 3;

    private Coroutine _createUnitsCoroutine;

    private void OnEnable()
    {
        _radar.ResoursesFound += _pickingObjectsService.AddToList;
        _pickingObjectsService.ListUpdated += OnResourseFound;
    }

    private void OnDisable()
    {
        _radar.ResoursesFound -= _pickingObjectsService.AddToList;
        _pickingObjectsService.ListUpdated -= OnResourseFound;
    }

    private void Start()
    {
        if (_createUnitsCoroutine != null)
        {
            StopCoroutine(_createUnitsCoroutine);
        }

        if (_freeUnits.Count < _startCount)
            _createUnitsCoroutine = StartCoroutine(CreateUnits());
    }

    private void OnDestroy()
    {
        if (_createUnitsCoroutine != null)
            StopCoroutine(_createUnitsCoroutine);
    }

    public void Initialize(ResoursesSpawner resourseSpawner)
    {
        _resourseSpawner = resourseSpawner;
    }

    private void OnValidate()
    {
        _radar ??= GetComponent<Radar>();
    }

    private IEnumerator CreateUnits()
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

    private void SendUnit(PickingObject pickingObject)
    {
        pickingObject.Dropped += _storage.IncreaseCount;
        pickingObject.ReadyToBackToPull += _pickingObjectsService.RemoveFromList;

        foreach (var unit in _freeUnits)
        {
            if (unit.TryGetComponent<ObjectPicker>(out ObjectPicker picker))
            {
                if (picker.CurrentObject != null)
                {
                    return;
                }

                if (unit.TryGetComponent<UnitMover>(out UnitMover mover))
                {
                    unit.SendForResourse(pickingObject);
                    unit.BecameFree += MoveUnitToFree;
                    _pickingObjectsService.PutResourseInOcupiedList(pickingObject);
                    MoveUnitToOcupied(unit);
                    return;
                }
            }
        }
    }

    private void MoveUnitToFree(Unit unit)
    {
        unit.BecameFree -= MoveUnitToFree;
        _freeUnits.Add(unit);
        _ocupiedUnits.Remove(unit);
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
            SendUnit(resourse);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Base : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private Storage _storage;
    [SerializeField] private Radar _radar;
    [SerializeField] private PickingObjectsService _pickingObjectsService;
    [SerializeField] private Transform _watingZone;
    [SerializeField] private SelectableObject _selectableObject;
    [SerializeField] private Flag _flag;
    [SerializeField] private CameraRay _cameraRay;
    [SerializeField] private Player _player;

    private List<Unit> _ocupiedUnits = new List<Unit>();
    private List<Unit> _freeUnits = new List<Unit>();

    private float _delay = 3f;
    private int _startCount = 3;
    private int _unitCost = 3;

    private Coroutine _createUnitsCoroutine;

    public int UnitCost => _unitCost;

    private void OnEnable()
    {
        _radar.ResoursesFound += _pickingObjectsService.AddToList;
        _pickingObjectsService.ListUpdated += OnResourseFound;
        _storage.IsEnoughForUnit += CreateUnit;
        _selectableObject.Selected += TurnOnFlag;
     //   _player.LMBPressed += SetFlag;
    }

    private void OnDisable()
    {
        _radar.ResoursesFound -= _pickingObjectsService.AddToList;
        _pickingObjectsService.ListUpdated -= OnResourseFound;
        _storage.IsEnoughForUnit -= CreateUnit;
        _selectableObject.Selected -= TurnOnFlag;
      //  _player.LMBPressed -= SetFlag;
    }

    private void Start()
    {
        if (_createUnitsCoroutine != null)
        {
            StopCoroutine(_createUnitsCoroutine);
        }

        if (_freeUnits.Count < _startCount)
            _createUnitsCoroutine = StartCoroutine(CreateStartUnits());
    }

    private void Update()
    {
        if (_flag.Selected)
        {
            _flag.transform.position = _cameraRay.GroundPoint;
        }
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

    private void TurnOnFlag()
    {
        _flag.gameObject.SetActive(true);
        _flag.MakeSelected();
    }

    private void SetFlag()
    {
        if (_selectableObject.ObjectSelected)
        {
            if (_flag.Selected)
            {
                _flag.MakeUnselected();
            }
        }
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

    private void SendUnit(PickingObject pickingObject)
    {
        pickingObject.ReadyToBackToPull += RemoveFromList;

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
            SendUnit(resourse);
        }
    }
}

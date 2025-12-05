using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private List<Unit> _freeUnits;
    [SerializeField] private List<Unit> _ocupiedUnits;
    [SerializeField] private Storage _storage;
    [SerializeField] private StorageCollision _storageCollision;
    [SerializeField] private Transform _watingZone;
    [SerializeField] private Radar _radar;
    [SerializeField] private PickingObjectsService _pickingObjectsService;

    private float _delay = 3f;
    private int _startCount = 3;

    private Coroutine _createUnitsCoroutine;
    private Coroutine _sendUnitsCoroutine;

    private void OnEnable()
    {
        _radar.ResoursesFound += _pickingObjectsService.FillList;
        _storageCollision.ResourseDropped += _pickingObjectsService.RemoveFromList;
    }

    private void OnDisable()
    {
        _radar.ResoursesFound -= _pickingObjectsService.FillList;
        _storageCollision.ResourseDropped -= _pickingObjectsService.RemoveFromList;
    }

    private void Start()
    {
        if (_freeUnits.Count < _startCount)
            _createUnitsCoroutine = StartCoroutine(CreateUnits());
        _sendUnitsCoroutine = StartCoroutine(SendUnitsCoroutine());
    }

    private void OnDestroy()
    {
        if (_createUnitsCoroutine != null)
            StopCoroutine(_createUnitsCoroutine);
        if (_sendUnitsCoroutine != null)
            StopCoroutine(_sendUnitsCoroutine);
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

            if (unit.TryGetComponent<UnitMover>(out UnitMover mover))
            {
                mover.GoToTarget(_watingZone.transform.position);
                _freeUnits.Add(unit);

                yield return delay;
            }
        }
    }



    private IEnumerator SendUnitsCoroutine()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        while (enabled)
        {
            yield return delay;

            List<PickingObject> pickingObjects = _pickingObjectsService.GetPickingObjects();

            if (pickingObjects.Count > 0)
            {
                for (int i = 0; i < pickingObjects.Count; i++)
                {
                    SendForResourse(pickingObjects[i]);
                }
            }
        }
    }

    private void SendForResourse(PickingObject pickingObject)
    {
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
                    _pickingObjectsService.PutResourseInOcupiedList(pickingObject);
                    _ocupiedUnits.Add(unit);
                    _freeUnits.Remove(unit);
                    return;
                }
            }
        }
    }
}

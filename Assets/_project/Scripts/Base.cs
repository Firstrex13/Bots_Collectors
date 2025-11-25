using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private Player _player;
    [SerializeField] private Storage _storage;
    [SerializeField] private StorageCollision _storageCollision;
    [SerializeField] private Transform _watingZone;
    [SerializeField] private Radar _radar;
    [SerializeField] private PickingObjectsService _pickingObjectsService;

    private float _delay = 2f;
    private int _startCount = 3;

    private Coroutine _createUnitsCoroutine;

    private void OnEnable()
    {
        _pickingObjectsService.ListUpdated += SendForResourse;
        _storageCollision.ResourseDropped += _pickingObjectsService.RemoveResourseFromList;
    }

    private void OnDisable()
    {
        _pickingObjectsService.ListUpdated -= SendForResourse;
        _storageCollision.ResourseDropped -= _pickingObjectsService.RemoveResourseFromList;
    }

    private void Start()
    {
        if (_units.Count < _startCount)
            _createUnitsCoroutine = StartCoroutine(CreateUnits());
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

    private IEnumerator CreateUnits()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        for (int i = 0; i < _startCount; i++)
        {
            Unit unit = _unitSpawner.Create(_spawnPoint);

            unit.GoToTarget(_watingZone.transform.position);
            _units.Add(unit);

            yield return delay;
        }
    }

    public void SendUnitBack(Unit unit)
    {
        unit.GoToTarget(_storage.transform.position);
        unit.ReadyGoToStorage -= SendUnitBack;
    }

    public void SendUnitToWaitingZone(Unit unit)
    {
        unit.GoToTarget(_watingZone.transform.position);
        unit.BecameFree -= SendUnitToWaitingZone;
    }

    private void SendForResourse(PickingObject pickingObject)
    {
        if (pickingObject != null)
        {
            foreach (var unit in _units)
            {
                if (unit.TryGetComponent<ObjectPicker>(out ObjectPicker picker))
                {
                    if (picker.CurrentObject != null)
                    {
                        return;
                    }

                    unit.GoToTarget(pickingObject.transform.position);
                    _pickingObjectsService.PutResourseInOcupiedList(pickingObject);
                    unit.ReadyGoToStorage += SendUnitBack;
                    unit.BecameFree += SendUnitToWaitingZone;
                    return;
                }

            }
        }
    }
}

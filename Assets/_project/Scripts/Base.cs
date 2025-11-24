using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private PickingObjectsService _pickingObjectService;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private Player _player;
    [SerializeField] private Storage _storage;
    [SerializeField] private Transform _watingZone;
    [SerializeField] private ResoursesSpawner _resourseSpawner;

    private float _delay = 2f;
    private int _startCount = 3;

    private Coroutine _createUnitsCoroutine;

    private void OnEnable()
    {
        _resourseSpawner.Created += SendForResourse;
    }

    private void OnDisable()
    {
        _resourseSpawner.Created -= SendForResourse;
    }

    private void Start()
    {
        if(_units.Count < _startCount)
        _createUnitsCoroutine = StartCoroutine(nameof(CreateUnits));
    }

    private void OnDestroy()
    {
        if(_createUnitsCoroutine != null)
        StopCoroutine(_createUnitsCoroutine);
    }

    private void OnValidate()
    {
        _resourseSpawner ??= GetComponent<ResoursesSpawner>();
        _pickingObjectService ??= GetComponent<PickingObjectsService>();
    }

    private IEnumerator CreateUnits()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        for (int i = 0; i < _startCount; i++)
        {
            Unit unit = _unitSpawner.Create(_spawnPoint);

            unit.Initialize();
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

    private void SendForResourse()
    {
        List<PickingObject> pickingObjects = _pickingObjectService.GetPickingObjectsFree();

        if (pickingObjects != null)
        {
            foreach (var item in pickingObjects)
            {
                foreach (var unit in _units)
                {
                    if (unit.IsFree && !item.Aimed)
                    {
                        unit.GoToTarget(item.transform.position);
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

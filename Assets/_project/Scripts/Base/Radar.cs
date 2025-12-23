using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private float _period;

    private Coroutine _scan;

    private Collider[] _colliders = new Collider[20];

    public event Action<List<PickingObject>> ResoursesFound;

    private void Start()
    {
        _scan = StartCoroutine(Scan());
    }

    private void OnDestroy()
    {
        StopCoroutine(_scan);
    }

    private IEnumerator Scan()
    {
        WaitForSeconds delay = new WaitForSeconds(_period);

        while (enabled)
        {
            yield return delay;
            int count = Physics.OverlapSphereNonAlloc(transform.position, _radius, _colliders, _mask);

            List<PickingObject> pickingObjects = new List<PickingObject>();

            for (int i = 0; i < count; ++i)
            {
                if (_colliders[i].TryGetComponent(out PickingObject pickingObject))
                {
                    pickingObjects.Add(pickingObject);
                }
            }

            ResoursesFound?.Invoke(pickingObjects);
            Debug.Log("Scannig");
        }
    }
}

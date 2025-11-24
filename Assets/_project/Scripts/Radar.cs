using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _mask;

    private Collider[] _colliders = new Collider[20];

    public List<PickingObject> GetScanedObjects()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, _radius, _colliders, _mask);

        PickingObject pickingObject;
        List<PickingObject> pickingObjects = new List<PickingObject>();

        for (int i = 0; i < count; ++i)
        {
            if (_colliders[i].TryGetComponent<PickingObject>(out pickingObject))
            {
                if (!pickingObject.Scanned)
                {
                    pickingObjects.Add(pickingObject);
                    pickingObject.MakeObjectScanned();
                    return pickingObjects;
                }
            }
        }

        return null;
    }
}

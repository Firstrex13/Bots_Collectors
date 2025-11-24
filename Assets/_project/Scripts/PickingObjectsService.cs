using System.Collections.Generic;
using UnityEngine;

public class PickingObjectsService : MonoBehaviour
{
    [SerializeField] private Radar _radar;

    private List<PickingObject> _pickingObjects = new List<PickingObject>();

    public List<PickingObject> GetPickingObjectsFree()
    {
        _pickingObjects = _radar.GetScanedObjects();

        return _pickingObjects;
    }
}

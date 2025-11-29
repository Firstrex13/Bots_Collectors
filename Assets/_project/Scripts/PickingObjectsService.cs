using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickingObjectsService : MonoBehaviour
{
    [SerializeField] private ResoursesSpawner _resourseSpawner;

    [SerializeField] private List<PickingObject> _pickingObjectsFree = new List<PickingObject>();
    [SerializeField] private List<PickingObject> _pickingObjectsOcupaied = new List<PickingObject>();

    public event Action<PickingObject> ListUpdated;

    public void FillList(PickingObject pickingObject)
    {
        if (_pickingObjectsFree.Contains(pickingObject))
        {
            return;
        }

        if (_pickingObjectsOcupaied.Contains(pickingObject))
        {
            return;
        }

        _pickingObjectsFree.Add(pickingObject);
        ListUpdated?.Invoke(pickingObject);
    }

    public void PutResourseInOcupiedList(PickingObject resourse)
    {
        _pickingObjectsOcupaied.Add(resourse);
        _pickingObjectsFree.Remove(resourse);
    }

    public void RemoveFromList(PickingObject pickingObject)
    {
        _pickingObjectsOcupaied.Remove(pickingObject);
    }

    public List<PickingObject> GetPickingObjects()
    {
        return _pickingObjectsFree.ToList();
    }
}


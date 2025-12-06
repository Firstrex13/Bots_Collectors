using System;
using System.Collections.Generic;
using UnityEngine;

public class PickingObjectsService : MonoBehaviour
{
    [SerializeField] private List<PickingObject> _pickingObjectsFree = new List<PickingObject>();
    [SerializeField] private List<PickingObject> _pickingObjectsOcupaied = new List<PickingObject>();

    public event Action ListUpdated;

    public void AddToList(PickingObject pickingObject)
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
        ListUpdated?.Invoke();
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

    public PickingObject GetFreeObject()
    {
        if(_pickingObjectsFree[0] == null)
        {
            return null;
        }

        return _pickingObjectsFree[0];
    }
}


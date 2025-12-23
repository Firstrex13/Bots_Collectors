using System;
using System.Collections.Generic;
using UnityEngine;

public class PickingObjectsService : MonoBehaviour
{
    [SerializeField] private List<PickingObject> _pickingObjectsFree = new List<PickingObject>();
    [SerializeField] private List<PickingObject> _pickingObjectsOcupaied = new List<PickingObject>();

    public event Action ListUpdated;

    public void AddToList(List<PickingObject> pickingObjects)
    {
        for (int i = 0; i < pickingObjects.Count; i++)
        {
            if (_pickingObjectsFree.Contains(pickingObjects[i]))
            {
                return;
            }

            if (_pickingObjectsOcupaied.Contains(pickingObjects[i]))
            {
                return;
            }


            _pickingObjectsFree.Add(pickingObjects[i]);
            ListUpdated?.Invoke();
        }
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

    public PickingObject GetFreeObjects()
    {
        if (_pickingObjectsFree.Count < 1)
        {
            return null;
        }

        return _pickingObjectsFree[0];
    }
}


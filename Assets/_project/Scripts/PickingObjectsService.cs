using System;
using System.Collections.Generic;
using UnityEngine;

public class PickingObjectsService : MonoBehaviour
{
    [SerializeField] private Radar _radar;
    [SerializeField] private ResoursesSpawner _resourseSpawner;

    [SerializeField] private List<PickingObject> _pickingObjectsFree = new List<PickingObject>();
    [SerializeField] private List<PickingObject> _pickingObjectsOcupaied = new List<PickingObject>();

    public event Action<PickingObject> ListUpdated;

    private void OnEnable()
    {
        _radar.ResoursesFound += FillList;
        _resourseSpawner.Returned += PutResourseInOcupiedList;
    }

    private void OnDisable()
    {
        _radar.ResoursesFound -= FillList;
        _resourseSpawner.Returned -= PutResourseInOcupiedList;
    }

    public void FillList(PickingObject pickingObject)
    {
        if (_pickingObjectsFree.Contains(pickingObject))
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

    public void RemoveResourseFromList(PickingObject pickingObject)
    {
        _pickingObjectsOcupaied.Remove(pickingObject);
    }
}


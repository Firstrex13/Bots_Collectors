using System;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private List<Resourse> _resourses;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private ResoursesSpawner _resourseSpawner;

    private Collider[] _colliders = new Collider[20];

    public event Action ResourseFound;

    public List<Resourse> Resourses => _resourses;

    private void OnEnable()
    {
        _resourseSpawner.Created += ScanArea;
    }

    private void OnDisable()
    {
        _resourseSpawner.Created -= ScanArea;
    }

    public void ScanArea()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, _radius, _colliders, _mask);

        Resourse resourse;

        for (int i = 0; i < count; ++i)
        {
            if (_colliders[i].TryGetComponent<Resourse>(out resourse))
            {
                if (!resourse.Scanned)
                {
                    _resourses.Add(resourse);
                    resourse.MakeObjectScanned();
                    ResourseFound?.Invoke();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private List<Resourse> _resourses;
    [SerializeField] private LayerMask _mask;

    private Collider[] _colliders = new Collider[20];

    public event Action ResourseFound;
    public List<Resourse> Resourses => _resourses;
  
    public void OnScanArea()
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

using System;
using UnityEngine;

public class Selector : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Base _base;

    public event Action<Base> Selected;

    public Base Base => _base;

    private void OnEnable()
    {
        _player.LMBPressed += PlaceFlag;
    }

    private void OnDisable()
    {
        _player.LMBPressed -= PlaceFlag;
    }

    public void MakeHovered(Base baseItem)
    {
        _base = baseItem;
    }

    public void MakeUnhovered()
    {
        if (_base != null)
        {
            _base = null;
        }
    }

    private void PlaceFlag()
    {
        Selected?.Invoke(_base);
        _base = null;
    }
}

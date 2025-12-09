using System;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [SerializeField] private bool _hovered;
    [SerializeField] private Player _player;

    private bool _selected;

    public bool ObjectSelected => _selected; 

    public event Action Selected;

    private void OnEnable()
    {
        _player.LMBPressed += SelectBase;
    }

    private void OnDisable()
    {
        _player.LMBPressed -= SelectBase;
    }

    public void MakeHovered()
    {
        _hovered = true;
    }

    public void MakeUnhovered()
    {
        _hovered = false;
    }

    private void SelectBase()
    {
        if (_hovered)
        {
            _selected = true;
            Selected?.Invoke();
        }
    }

    public void Unselect()
    {
        _selected = false;
    }

    
}

using System;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [SerializeField] private bool _hovered;
    [SerializeField] private Player _player;

    public event Action Selected;

    public bool Hovered => _hovered;

    private void OnEnable()
    {
        _player.LMBPressed += PlaceFlag;
    }

    private void OnDisable()
    {
        _player.LMBPressed -= PlaceFlag;
    }

    public void MakeHovered()
    {
        _hovered = true;
    }

    public void MakeUnhovered()
    {
        _hovered = false;
    }

    private void PlaceFlag()
    {
        Selected?.Invoke();
    }
}

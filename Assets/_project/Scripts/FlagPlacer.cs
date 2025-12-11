using System;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private Flag _flag;
    [SerializeField] private SelectableObject _selectableObject;
    [SerializeField] private CameraRay _cameraRay;
    [SerializeField] private Player _player;

    [SerializeField] private bool _selected;

    public event Action<Vector3> FlagPlaced;
    public bool Selected => _selected;

    private void OnEnable()
    {
        _selectableObject.Selected += SetFlag;
    }

    private void TurnOnFlag()
    {
        _flag.gameObject.SetActive(true);
        _selected = true;

    }

    private void TurnOffFlag()
    {
        _flag.gameObject.SetActive(false);
        _selected = false;
        _flag.transform.position = Vector3.zero;
    }

    private void SetFlag()
    {
        if (_selectableObject.Hovered)
        {
            if (!_selected)
            {
                TurnOnFlag();
            }
            else
            {
                TurnOffFlag();
            }
        }
        else if (!_selectableObject.Hovered)
        {
            if (_selected)
            {
                _flag.transform.position = _cameraRay.GroundPoint;
                FlagPlaced?.Invoke(_flag.transform.position);
            }
        }

        _player.LMBPressed -= SetFlag;
    }
}

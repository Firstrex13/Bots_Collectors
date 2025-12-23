using System;
using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private Selector _selector;
    [SerializeField] private CameraRay _cameraRay;
    [SerializeField] private Player _player;
    [SerializeField] private bool _selected;

    private Flag _flag;
    private Base _base;

    public event Action FlagPlaced;
    public bool Selected => _selected;

    private void OnEnable()
    {
        _selector.Selected += SetFlag;
    }

    private void OnDisable()
    {
        _selector.Selected -= SetFlag;
    }

    private void Update()
    {
        if (_selected)
        {
            _flag.transform.position = _cameraRay.GroundPoint;
        }
    }

    private void SetFlag(Base baseItem)
    {
        if (baseItem != null)
        {
            _base = baseItem;
            _flag = _base.Flag;
        }

        if (_selector.Base != null)
        {
            if (!_selected)
            {
                _flag.TurnOn();
                _selected = true;
            }
            else
            {
                _flag.TurnOff();
                _selected = false;
            }
        }
        else
        {
            if (_selected)
            {
                BaseStates baseStates;
                if (_base.TryGetComponent(out baseStates))
                {
                    baseStates.ChangeState();
                    _selected = false;
                    FlagPlaced?.Invoke();
                }
            }
        }
    }
}

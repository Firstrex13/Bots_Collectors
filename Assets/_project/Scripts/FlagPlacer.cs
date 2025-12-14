using System;
using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private Selector _selector;
    [SerializeField] private CameraRay _cameraRay;
    [SerializeField] private Player _player;
    [SerializeField] private bool _selected;

    private Flag _flag;

    public event Action<Vector3> FlagPlaced;
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

    private void TurnOnFlag(Flag flag)
    {
        Debug.Log("TurnedOn");
        flag.gameObject.SetActive(true);
        _selected = true;
    }

    private void TurnOffFlag(Flag flag)
    {
        Debug.Log("TurnedOff");
        flag.gameObject.SetActive(false);
        _selected = false;
        flag.transform.position = Vector3.zero;
    }

    private void SetFlag(Base baseItem)
    {
        Debug.Log("123");

        if (baseItem != null)
        {
            _flag = baseItem.Flag;
        }

        if (_selector.Base != null)
        {
            if (!_selected)
            {
                TurnOnFlag(_flag);
            }
            else
            {
                TurnOffFlag(_flag);
            }
        }
        else
        {
            if (_selected)
            {
                Debug.Log("Placed");
                _selected = false;
                FlagPlaced?.Invoke(_flag.transform.position);
            }
        }
    }
}

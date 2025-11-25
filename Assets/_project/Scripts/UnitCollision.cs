using UnityEngine;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(ObjectPicker))]

public class UnitCollision : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private ObjectPicker _objectPicker;

    private void OnValidate()
    {
        _unit ??= GetComponent<Unit>();
        _objectPicker ??= GetComponent<ObjectPicker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PickingObject>(out PickingObject pickingObject))
        {
            if (_objectPicker.CurrentObject == null)
            {
                _unit.PickUpObject(other);
                _unit.SendReadyToGoStorageEvent();
            }
        }

        if (other.TryGetComponent<Storage>(out _) && _objectPicker.CurrentObject != null)
        {
            _unit.DropObject();
            _unit.SendMessageBecameFreeEvent();
        }
    }
}

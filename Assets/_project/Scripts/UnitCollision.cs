using UnityEngine;

[RequireComponent(typeof(Unit))]

public class UnitCollision : MonoBehaviour
{
    [SerializeField] private Unit _unit;

    private void OnValidate()
    {
        _unit ??= GetComponent<Unit>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PickingObject>(out PickingObject pickingObject))
        {
            if (pickingObject.Collected == false && _unit.IsHolding == false)
            {
                _unit.PickUpObject(other);
                pickingObject.MakeCollected();
                _unit.SendReadyToGoStorageEvent();
                _unit.MakeIsHolding();
            }

        }

        if (other.TryGetComponent<Storage>(out _) && _unit.IsHolding)
        {
            _unit.DropObject();
            _unit.SendMessageBecameFreeEvent();
            _unit.MakeNotIsHolding();
        }
    }
}

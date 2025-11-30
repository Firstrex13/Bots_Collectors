using System;
using UnityEngine;

public class StorageCollision : MonoBehaviour
{
    [SerializeField] private Storage _storage;

    public event Action<PickingObject> ResourseDropped;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ObjectPicker>(out ObjectPicker picker))
        {
            if (picker.CurrentObject == null)
            {
                return;
            }

            if (picker.CurrentObject)
            {
                if (picker.CurrentObject.TryGetComponent<PickingObject>(out PickingObject pickingObject))
                {
                    picker.Drop();
                    _storage.IncreaseCount();
                    ResourseDropped?.Invoke(pickingObject);
                }
            }
        }
    }
}

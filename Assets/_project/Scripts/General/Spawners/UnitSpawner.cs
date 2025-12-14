using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _prefab;
    [SerializeField] private BaseBuilder _baseBuilder;

    public Unit Create(Transform position)
    {
        Unit unit = Instantiate(_prefab, position.position, transform.rotation, transform);
        unit.Initialize(_baseBuilder);
        return unit;
    }
}

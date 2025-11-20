using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _prefab;

    public void Create(Transform position)
    {
        Unit unit = Instantiate(_prefab, position.position, transform.rotation);

        unit.Initialize();

    }
}

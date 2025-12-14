using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private Player _player;
    [SerializeField] private FlagPlacer _flagPacer;
    [SerializeField] private UnitSpawner _unitSpawner;

    public void BuildNewBase(Vector3 position, Unit unit)
    {
        Base newBase = Instantiate(_basePrefab, new Vector3(position.x, 0.55f, position.z), Quaternion.identity);

        newBase.Initialize(_flagPacer, _unitSpawner);

        newBase.ClearList();
        newBase.AddUnit(unit);
    }
}

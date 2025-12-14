using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private Base _basePrefab;
    [SerializeField] private Player _player;
    [SerializeField] private FlagPlacer _flagPacer;

    private void OnEnable()
    {
        _base.BaseBuildRequested += BuildNewBase;
    }

    private void BuildNewBase(Vector3 position, Unit unit)
    {
        Base newBase = Instantiate(_basePrefab, new Vector3(position.x, 0.55f, position.z), Quaternion.identity);

        newBase.Initialize(_flagPacer);

        newBase.ClearList();
        newBase.AddUnit(unit);
    }
}

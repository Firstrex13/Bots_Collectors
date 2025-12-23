using UnityEngine;

public class BaseStates : MonoBehaviour
{
    public enum State
    {
        BuildingUnits,
        BuildingNewBase
    }

    [SerializeField] private State _currentState;

    public State CurrentState => _currentState;

    private void Start()
    {
        _currentState = State.BuildingUnits;
    }

    public void ChangeState()
    {
        if (_currentState != State.BuildingNewBase)
        {
            _currentState = State.BuildingNewBase;
        }
        else
        {
            _currentState = State.BuildingUnits;
        }
    }
}

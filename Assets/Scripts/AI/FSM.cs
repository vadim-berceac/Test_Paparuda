
using UnityEngine;

public class FSM
{
    private readonly CharacterCore _character;
    private readonly AIInput _aiInput;
    private readonly CharacterStatesContainer _characterStatesContainer;
    private readonly StateTimer _stateTimer;
    private readonly NavMeshRandomPointFinder _pointFinder;
    
    public State CurrentState { get; private set; }
    public BehaviourType BehaviourType => _aiInput.BehaviourType;
    public Transform CurrentTarget => _aiInput.CurrentTarget;
    public Vector3 PatrolPoint = Vector3.zero;

    public FSM(
        CharacterCore character,
        AIInput aiInput,
        CharacterStatesContainer characterStatesContainer,
        StateType initialStateType = StateType.Idle
        )
    {
        _character = character;
        _aiInput = aiInput;
        _characterStatesContainer = characterStatesContainer;
       _stateTimer = new StateTimer();
        _pointFinder = new NavMeshRandomPointFinder(10, 5);
        
        SetState(initialStateType);
        CurrentState.OnEnter(this);
    }

    public void CreatePatrolPoint()
    {
        if (!_pointFinder.TryFindPoint(_character.transform.position, 20, out var newPoint))
        {
            PatrolPoint = _character.transform.position; 
            return;
        }
    
        PatrolPoint = newPoint;
    }

    public AIInput GetAIInput()
    {
        return _aiInput;
    }

    public StateTimer GetStateTimer()
    {
        return _stateTimer;
    }

    public void SetState(StateType stateType)
    {
        CurrentState = _characterStatesContainer.GetState(stateType);
    }

    public void OnUpdate()
    {
        CurrentState.OnUpdate(this);
    }

    public void OnFixedUpdate()
    {
        CurrentState.OnFixedUpdate(this);
    }

    public void OnLateUpdate()
    {
        CurrentState.OnLateUpdate(this);
    }
}

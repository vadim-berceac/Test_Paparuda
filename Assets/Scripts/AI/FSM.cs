
public class FSM
{
    private readonly CharacterCore _character;
    private readonly AIInput _aiInput;
    private readonly CharacterStatesContainer _characterStatesContainer;
    
    public State CurrentState { get; private set; }
    public BehaviourType BehaviourType => _aiInput.BehaviourType;

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
        SetState(characterStatesContainer.GetState(initialStateType));
    }

    public void SetState(State state)
    {
        CurrentState = state;
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

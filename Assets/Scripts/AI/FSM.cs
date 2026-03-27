
public class FSM
{
    private readonly CharacterCore _character;
    public State CurrentState { get; private set; }

    public FSM(State initialState, CharacterCore character)
    {
        CurrentState = initialState;
        
        _character = character;
    }

    public void SetState(State state)
    {
        CurrentState = state;
    }

    public void Update()
    {
        CurrentState.Update(this);
    }

    public void FixedUpdate()
    {
        CurrentState.FixedUpdate(this);
    }

    public void LateUpdate()
    {
        CurrentState.LateUpdate(this);
    }
}

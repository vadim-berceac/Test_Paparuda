
public abstract class State
{
    protected virtual void Enter(FSM machine) { }
    protected virtual void Exit(FSM machine) { }
    public virtual void Update(FSM machine) { }
    public virtual void FixedUpdate(FSM machine) { }
    public virtual void LateUpdate(FSM machine) { }
    protected virtual void CheckSwitch(FSM machine) { }

    protected void SwitchState(FSM machine, State newState)
    {
        machine.CurrentState?.Exit(machine);
        machine.SetState(newState);
        machine.CurrentState?.Enter(machine);
    }
}

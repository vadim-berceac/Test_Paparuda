
using UnityEngine;

public abstract class State : ScriptableObject
{
    [field: SerializeField] public StateType StateType { get; private set; }
    protected virtual void OnEnter(FSM machine) { }
    protected virtual void OnExit(FSM machine) { }
    public virtual void OnUpdate(FSM machine) { }
    public virtual void OnFixedUpdate(FSM machine) { }
    public virtual void OnLateUpdate(FSM machine) { }
    protected virtual void OnCheckSwitch(FSM machine) { }

    protected void SwitchState(FSM machine, State newState)
    {
        machine.CurrentState?.OnExit(machine);
        machine.SetState(newState);
        machine.CurrentState?.OnEnter(machine);
    }
}

public enum StateType
{
    Idle,
    Chase,
    Flee,
    Patrol,
    Stun
}

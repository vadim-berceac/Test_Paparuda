
using UnityEngine;

public abstract class State : ScriptableObject
{
    [field: SerializeField] public StateType StateType { get; private set; }
    [field: SerializeField] public TimerSettings TimerSettings { get; private set; }

    public virtual void OnEnter(FSM machine)
    {
        if (TimerSettings.Enabled)
        {
            var time = Random.Range(TimerSettings.MinStateTime, TimerSettings.MaxStateTime);
            machine.GetStateTimer().SetTime(time);
        }
    }

    protected virtual void OnExit(FSM machine)
    {
        if (TimerSettings.Enabled)
        {
            machine.GetStateTimer().ResetTime();
        }
    }

    public virtual void OnUpdate(FSM machine)
    {
        if (TimerSettings.Enabled)
        {
            machine.GetStateTimer().OnUpdate();
        }
        
        OnCheckSwitch(machine);
    }
    public virtual void OnFixedUpdate(FSM machine) { }
    public virtual void OnLateUpdate(FSM machine) { }
    protected virtual void OnCheckSwitch(FSM machine) { }

    protected void SwitchState(FSM machine, StateType stateType)
    {
        machine.CurrentState?.OnExit(machine);
        machine.SetState(stateType);
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

[System.Serializable]
public struct TimerSettings
{
    [field: SerializeField] public bool Enabled { get; set; }
    [field: SerializeField, Range(0, 120)] public float MinStateTime { get; set; }
    [field: SerializeField, Range(0, 120)] public float MaxStateTime { get; set; }
}

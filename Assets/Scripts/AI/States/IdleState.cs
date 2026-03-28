using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "Scriptable Objects/States/IdleState")]
public class IdleState : State
{
    public override void OnUpdate(FSM machine)
    {
        base.OnUpdate(machine);

        if (machine.CurrentTarget)
        {
            machine.GetAIInput().SetMoveTarget(machine.CurrentTarget);
        }
    }

    protected override void OnCheckSwitch(FSM machine)
    {
        base.OnCheckSwitch(machine);

        if (machine.GetStateTimer().Reached)
        {
            SwitchState(machine, StateType.Patrol);
        }
    }
}

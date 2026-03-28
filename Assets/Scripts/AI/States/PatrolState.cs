using UnityEngine;

[CreateAssetMenu(fileName = "PatrolState", menuName = "Scriptable Objects/States/PatrolState")]
public class PatrolState : State
{
    public override void OnEnter(FSM machine)
    {
        base.OnEnter(machine);
        
        machine.CreatePatrolPoint();
        
        if (machine.PatrolPoint != Vector3.zero)
        {
            machine.GetAIInput().SetMoveTargetPosition(machine.PatrolPoint);
        }
    }

    protected override void OnCheckSwitch(FSM machine)
    {
        base.OnCheckSwitch(machine);

        if (machine.GetAIInput().IsTargetReached())
        {
            SwitchState(machine, StateType.Idle);
        }
    }

    protected override void OnExit(FSM machine)
    {
        base.OnExit(machine);
        machine.GetAIInput().StopMovement();
        machine.GetAIInput().StopRotation();
    }
}

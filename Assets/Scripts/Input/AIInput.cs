using UnityEngine;
using Zenject;

public class AIInput : MonoBehaviour, ICharacterInput, IEnableable
{
    [field: SerializeField] public BehaviourType BehaviourType { get; private set; }
    
    public Vector2 Move { get; set; }
    public Vector3 Rotation { get; set; }
    public bool Run { get; set; }
    public bool Interact { get; set; }
    public bool Attack { get; set; }

    public bool Enabled { get; set; }  
    private FSM _stateMachine;

    [Inject]
    private void Construct(
        CharacterCore characterCore,
        CharacterStatesContainer characterStatesContainer
        )
    {
        _stateMachine = new FSM(characterCore, this, characterStatesContainer);
    }

    public void Enable()
    {
        Enabled = true;
    }

    public void Disable()
    {
        Enabled = false;
    }

    private void Update()
    {
        if(!Enabled) return;
        _stateMachine.OnUpdate();
    }

    private void FixedUpdate()
    {
        if(!Enabled) return;
        _stateMachine.OnFixedUpdate();
    }

    private void LateUpdate()
    {
        if(!Enabled) return;
        _stateMachine.OnLateUpdate();
    }
}

public enum BehaviourType
{
    Aggressive = 0,
    Cowardly = 1,
}

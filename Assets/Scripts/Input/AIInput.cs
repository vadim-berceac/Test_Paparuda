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
    private VisionSystem _visionSystem;

    [Inject]
    private void Construct(
        CharacterCore characterCore,
        CharacterStatesContainer characterStatesContainer,
        VisionSystem visionSystem
        )
    {
        _stateMachine = new FSM(characterCore, this, characterStatesContainer);
        _visionSystem = visionSystem;
    }

    public void Enable()
    {
        Enabled = true;
        _visionSystem.Enable();
    }

    public void Disable()
    {
        Enabled = false;
        _visionSystem.Disable();
    }

    private void Update()
    {
        if(!Enabled) return;
        _stateMachine.OnUpdate();
        CheckTargets();
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

    private void CheckTargets()
    {
        if (_visionSystem.GetClosestLiveCharacter() == null) return;
        
        Debug.Log(_visionSystem.GetClosestLiveCharacter());
    }
}

public enum BehaviourType
{
    Aggressive = 0,
    Cowardly = 1,
}

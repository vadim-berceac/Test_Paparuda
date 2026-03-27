using UnityEngine;
using Zenject;

public class AIInput : MonoBehaviour, ICharacterInput
{
    [SerializeField] private BehaviourType behaviourType;
    
    public Vector2 Move { get; set; }
    public Vector3 Rotation { get; set; }
    public bool Run { get; set; }
    public bool Interact { get; set; }
    public bool Attack { get; set; }

    private bool _enabled;
    private FSM _stateMachine;
    //private StatesContainer _container;

    [Inject]
    private void Construct(CharacterCore characterCore)
    {
        //_stateMachine = new FSM("blank", characterCore);
    }

    public void Enable(bool enable)
    {
        _enabled = enable;
    }

    private void Update()
    {
        if(!_enabled) return;
        //_stateMachine.Update();
    }

    private void FixedUpdate()
    {
        if(!_enabled) return;
        //_stateMachine.FixedUpdate();
    }

    private void LateUpdate()
    {
        if(!_enabled) return;
        //_stateMachine.LateUpdate();
    }
}

public enum BehaviourType
{
    Aggressive = 0,
    Cowardly = 1,
}

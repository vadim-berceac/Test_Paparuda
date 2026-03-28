using UnityEngine;
using Zenject;

public class AIInput : MonoBehaviour, ICharacterInput, IEnableable
{
    [field: SerializeField] public BehaviourType BehaviourType { get; private set; }
    [field: SerializeField] public float StoppingDistance { get; private set; } = 0.5f;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 10f;
    
    public Vector2 Move { get; set; }
    public Vector3 Rotation { get; set; }
    public bool Run { get; set; }
    public bool Interact { get; set; }
    public bool Attack { get; set; }

    public bool Enabled { get; set; }
    
    private FSM _stateMachine;
    private VisionSystem _visionSystem;
    private Vector3? _targetPosition;
    private Vector3? _currentMoveDirection;
    
    public Transform CurrentTarget => _visionSystem.GetClosestLiveCharacter()?.Transform;

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

    public void SetMoveTarget(Transform targetTransform)
    {
        _targetPosition = targetTransform.position;
        var directionToTarget = (targetTransform.position - transform.position).normalized;
        
        _currentMoveDirection = directionToTarget;
       
        SetRotationFromDirection(directionToTarget);
    }

    public void SetMoveTargetPosition(Vector3 worldPosition)
    {
        _targetPosition = worldPosition;
        var directionToTarget = (worldPosition - transform.position).normalized;
        
        _currentMoveDirection = directionToTarget;
        
        SetRotationFromDirection(directionToTarget);
    }

    private void SetRotationFromDirection(Vector3 direction)
    {
        var yAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Rotation = new Vector3(0, yAngle, 0);
    }

    public bool IsTargetReached()
    {
        if (_targetPosition == null) return false;
        
        var distanceToTarget = (transform.position - _targetPosition.Value).magnitude;
        return distanceToTarget <= StoppingDistance;
    }

    public void StopMovement()
    {
        Move = Vector2.zero;
        _targetPosition = null;
        _currentMoveDirection = null;
    }

    public void StopRotation()
    {
        Rotation = Vector3.zero;
    }

    public void StopAllInput()
    {
        StopMovement();
        StopRotation();
        Run = false;
        Attack = false;
        Interact = false;
    }

    private void Update()
    {
        if(!Enabled) return;
        
        if (_currentMoveDirection.HasValue)
        {
            var localDirection = transform.InverseTransformDirection(_currentMoveDirection.Value);
            Move = new Vector2(localDirection.x, localDirection.z).normalized;
        }
       
        if (Rotation != Vector3.zero)
        {
            ApplyRotation();
        }
        
        _stateMachine.OnUpdate();
        CheckTargets();
    }

    private void ApplyRotation()
    {
        var targetRot = Quaternion.Euler(0f, Rotation.y, 0f);

        var angleDiff = Quaternion.Angle(transform.rotation, targetRot);

        if (angleDiff < 0.1f)
        {
            transform.rotation = targetRot;
            return;
        }

        var maxAngleDelta = RotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            maxAngleDelta
        );
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
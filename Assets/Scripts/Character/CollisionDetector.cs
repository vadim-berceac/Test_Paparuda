using UnityEngine;
using Zenject;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField] private CollisionSettings settings;
    
    private Animator _animator;
    private Vector3 _desiredDelta;
    private Transform _cashedTransform;

    [Inject]
    private void Construct(Animator animator)
    {
        _animator = animator;
        _cashedTransform = transform;
    }
    
    private void OnAnimatorMove()
    {
        _desiredDelta = _animator.deltaPosition;
        
        if (Physics.SphereCast(
                _cashedTransform.position, 
                settings.Radius, 
                _desiredDelta.normalized,
                out var hit,
                _desiredDelta.magnitude,
                settings.LayerMask
            ))
        {
            var safeDistance = hit.distance - settings.Radius * 0.1f;
            _desiredDelta = _desiredDelta.normalized * Mathf.Max(0, safeDistance);
        }
    
        _cashedTransform.position += _desiredDelta;
        _cashedTransform.rotation *= _animator.deltaRotation;
    }
}

[System.Serializable]
public struct CollisionSettings
{
    [field: SerializeField] public float Radius { get; private set; }
    [field: SerializeField] public LayerMask LayerMask { get; private set; }
}

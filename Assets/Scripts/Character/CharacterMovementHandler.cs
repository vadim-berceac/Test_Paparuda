using UnityEngine;

public class CharacterMovementHandler
{
    private const float RotationSpeed = 120f;
    
    private readonly Animator _animator;
    private readonly CharacterInputHandler _characterInputHandler;
    private readonly Transform _characterTransform;
    
    private readonly int _moveYHash;
    private readonly int _moveXHash;
    private readonly int _blendIndexHash;
    private readonly int _attackXHash;
    private readonly int _hitReactionHash;

    public CharacterMovementHandler
    (
        Animator animator,
        CharacterInputHandler characterInputHandler,
        Transform characterTransform
        )
    {
        _animator = animator;
        _characterInputHandler = characterInputHandler;
        _characterTransform = characterTransform;
        
        _moveXHash = Animator.StringToHash("MoveX");
        _moveYHash = Animator.StringToHash("MoveY");
        _blendIndexHash = Animator.StringToHash("BlendIndex");
        _attackXHash = Animator.StringToHash("Attack");
        _hitReactionHash = Animator.StringToHash("HitReaction");
    }

    public void UpdateMovement()
    {
        UpdateAnimator();
        
        if (_characterInputHandler.Rotation != Vector3.zero)
        {
            Rotation(_characterTransform, _characterInputHandler.Rotation, RotationSpeed);
        }
    }
    
    private void UpdateAnimator()
    {
        _animator.SetFloat(_moveXHash, _characterInputHandler.MoveInput.x);
        _animator.SetFloat(_moveYHash, _characterInputHandler.MoveInput.y);
        _animator.SetInteger(_blendIndexHash, GetBlendIndex());
        _animator.SetInteger(_attackXHash, _characterInputHandler.AttackPressed ? 1 : 0);
    }

    private int GetBlendIndex()
    {
        var hasMoveInput = Mathf.Abs(_characterInputHandler.MoveInput.y) > 0 
                           || Mathf.Abs(_characterInputHandler.MoveInput.x) > 0;
        var runIndex = _characterInputHandler.RunPressed ? 2 : 1;
        return hasMoveInput ? runIndex : 0;
    }
    
    private static void Rotation(Transform character, Vector3 rotation, float speed)
    {
        var targetRot = Quaternion.Euler(0f, rotation.y, 0f);

        var angleDiff = Quaternion.Angle(character.rotation, targetRot);

        if (angleDiff < 0.1f)
        {
            character.rotation = targetRot;
            return;
        }

        var maxAngleDelta = speed * Time.deltaTime;
        character.rotation = Quaternion.RotateTowards(
            character.rotation,
            targetRot,
            maxAngleDelta
        );
    }
}

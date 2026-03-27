
using UnityEngine;

public class CharacterInputHandler
{
    private readonly ICharacterInput _playerInput;
    private readonly ICharacterInput _aiInput;
    
    private const float MoveInputSmoothTime = 0.1f;
    
    private Vector2 _smoothedMoveInput;
    private Vector2 _rawMoveInput;
    private Vector2 _moveVelocity; 

    public Vector2 MoveInput => _smoothedMoveInput;
    public Vector3 Rotation => _currentInputSource?.Rotation ?? Vector3.zero;
    public bool RunPressed => _currentInputSource?.Run ?? false;
    public bool InteractPressed => _currentInputSource?.Interact ?? false;
    
    private ICharacterInput _currentInputSource;
    
    public CharacterInputHandler(ICharacterInput playerSource, ICharacterInput aiSource)
    {
        _playerInput = playerSource;
        _aiInput = aiSource;
        
        SetupInput(InputSourceMode.AI);
    }

    public void SetupInput(InputSourceMode mode)
    {
        switch (mode)
        {
            case InputSourceMode.None:
                _currentInputSource = null;
                _smoothedMoveInput = Vector2.zero;
                _moveVelocity = Vector2.zero;
                break;
            
            case InputSourceMode.AI:
                _currentInputSource = _aiInput;
                break;
            
            case InputSourceMode.Player:
                _currentInputSource = _playerInput;
                break;
        }
    }

    public void UpdateInput()
    {
        _rawMoveInput = _currentInputSource?.Move ?? Vector2.zero;
        
        if (_rawMoveInput.sqrMagnitude < 0.01f)
        {
            _smoothedMoveInput = Vector2.zero;
            _moveVelocity = Vector2.zero;
            return;
        }
    
        _smoothedMoveInput = Vector2.SmoothDamp(
            _smoothedMoveInput,
            _rawMoveInput,
            ref _moveVelocity,
            MoveInputSmoothTime
        );
    }
}

public enum InputSourceMode
{
    None = 0,
    AI = 1,
    Player = 2,
}

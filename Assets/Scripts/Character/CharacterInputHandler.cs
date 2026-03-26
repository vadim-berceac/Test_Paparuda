
using UnityEngine;

public class CharacterInputHandler
{
    private readonly ICharacterInput _playerInput;
    private readonly ICharacterInput _aiInput;

    public Vector2 MoveInput => _currentInputSource?.Move ?? Vector2.zero;
    public Vector3 Rotation => _currentInputSource?.Rotation ?? Vector3.zero;
    public bool JumpPressed => _currentInputSource?.Jump ?? false;
    public bool RunPressed => _currentInputSource?.Run ?? false;
    public bool InteractPressed => _currentInputSource?.Interact ?? false;
    
    private ICharacterInput _currentInputSource;
    
    public CharacterInputHandler(ICharacterInput playerSource, ICharacterInput aiSource)
    {
        _playerInput = playerSource;
        _aiInput = aiSource;
    }

    public void SetupInput(InputSourceMode mode)
    {
        switch (mode)
        {
            case InputSourceMode.None:
                _currentInputSource = null;
                break;
            
            case InputSourceMode.AI:
                _currentInputSource = _aiInput;
                break;
            
            case InputSourceMode.Player:
                _currentInputSource = _playerInput;
                break;
        }
    }
}

public enum InputSourceMode
{
    None = 0,
    AI = 1,
    Player = 2,
}

using System;
using Zenject;

public class CharacterSelector: IDisposable
{
    private readonly PlayerInput _playerInput;
    private readonly SceneCharacterContainer _sceneCharacterContainer;
    
    [Inject]
    private CharacterSelector(
        PlayerInput playerInput,
        SceneCharacterContainer sceneCharacterContainer
        )
    {
        _playerInput = playerInput;
        _sceneCharacterContainer = sceneCharacterContainer;

        _playerInput.OnCharacterSwitch += OnCharacterSelected;
    }

    private void OnCharacterSelected()
    {
        var characters = _sceneCharacterContainer.GetCharacters();

        foreach (var character in characters)
        {
            character.ResetPlayerSelection();
        }
        
        _sceneCharacterContainer.GetNextCharacter().SelectByPlayer();
    }
    
    public void Dispose()
    {
        _playerInput.OnCharacterSwitch -= OnCharacterSelected;
    }
}

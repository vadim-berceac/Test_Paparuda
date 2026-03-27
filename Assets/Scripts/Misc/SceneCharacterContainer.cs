using System.Collections.Generic;
using Zenject;

public class SceneCharacterContainer
{
    private readonly HashSet<CharacterCore> _characters = new HashSet<CharacterCore>();
    private CharacterCore _currentCharacter;
    
    [Inject]
    public SceneCharacterContainer() {}

    public CharacterCore GetNextCharacter()
    {
        if (_characters.Count == 0)
            return null;
    
        var list = new List<CharacterCore>(_characters);
    
        if (_currentCharacter == null || !_characters.Contains(_currentCharacter))
        {
            _currentCharacter = list[0];
            return _currentCharacter;
        }
    
        var currentIndex = list.IndexOf(_currentCharacter);
        var nextIndex = (currentIndex + 1) % list.Count;
        _currentCharacter = list[nextIndex];
        return _currentCharacter;
    }

    public HashSet<CharacterCore> GetCharacters()
    {
        return _characters;
    }

    public CharacterCore GetCurrentCharacter()
    {
        return _currentCharacter;
    }

    public void RegisterCharacter(CharacterCore character)
    {
        _characters.Add(character);
    }

    public void UnregisterCharacter(CharacterCore character)
    {
        _characters.Remove(character);
    }
}
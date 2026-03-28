using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStatesContainer", menuName = "Scriptable Objects/CharacterStatesContainer")]
public class CharacterStatesContainer : ScriptableObject
{
    [field: SerializeField] public State[] States { get; private set; }

    public State GetState(StateType stateType)
    {
        return States.FirstOrDefault(s => s.StateType == stateType);
    }
}
using UnityEngine;

public class AIInput : MonoBehaviour, ICharacterInput
{
    public Vector2 Move { get; set; }
    public Vector3 Rotation { get; set; }
    public bool Run { get; set; }
    public bool Interact { get; set; }
}

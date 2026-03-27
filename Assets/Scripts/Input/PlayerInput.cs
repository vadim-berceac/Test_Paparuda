using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[CreateAssetMenu(fileName = "PlayerInput", menuName = "Scriptable Objects/PlayerInput")]
public class PlayerInput : ScriptableObject, ITickable, ICharacterInput, ICameraInput
{
    [SerializeField] private InputActionAsset inputAsset;
    [SerializeField] private InputNames names;

    private InputAction _move;
    private InputAction _look;
    private InputAction _attack;
    private InputAction _run;
    private InputAction _interact;
    private InputAction _nextCharacter;

    private Transform _mainCamera;

    public Action OnCharacterSwitch;

    public Vector2 Move { get; set; }
    public Vector2 Look { get; set; }
    public Vector3 Rotation { get; set; }
    public bool Run { get; set; }
    public bool Interact { get; set; }
    public bool Attack { get; set; }

    [Inject]
    private void Construct(SceneCamera sceneCamera)
    {
        _mainCamera = sceneCamera.MainCamera;
        
        FindActions();
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    public void Tick()
    {
        if (_mainCamera != null)
            Rotation = _mainCamera.rotation.eulerAngles;
    }

    private void FindActions()
    {
        _move = inputAsset.FindAction(names.MoveActionName);
        _look = inputAsset.FindAction(names.LookActionName);
        _attack = inputAsset.FindAction(names.AttackActionName);
        _run = inputAsset.FindAction(names.RunActionName);
        _interact = inputAsset.FindAction(names.InteractActionName);
        _nextCharacter = inputAsset.FindAction(names.NextActionName);
    }

    private void Subscribe()
    {
        _move.performed += OnMoveCTX;
        _move.canceled += OnMoveCTXCancel;

        _look.performed += OnLookCTX;
        _look.canceled += OnLookCTXCancel;
        
        _run.performed += OnRunCTX;
        _run.canceled += OnRunCTXCancel;

        _attack.performed += OnAttackCTX;
        _attack.canceled += OnAttackCTXCancel;

        _interact.performed += OnInteractCTX;
        _interact.canceled += OnInteractCTXCancel;

        _nextCharacter.performed += OnNextCTX;
    }

    private void Unsubscribe()
    {
        _move.performed -= OnMoveCTX;
        _move.canceled -= OnMoveCTXCancel;

        _look.performed -= OnLookCTX;
        _look.canceled -= OnLookCTXCancel;

        _run.performed -= OnRunCTX;
        _run.canceled -= OnRunCTXCancel;
        
        _attack.performed -= OnAttackCTX;
        _attack.canceled -= OnAttackCTXCancel;

        _interact.performed -= OnInteractCTX;
        _interact.canceled -= OnInteractCTXCancel;

        _nextCharacter.performed -= OnNextCTX;
    }

    private void OnNextCTX(InputAction.CallbackContext ctx) => OnCharacterSwitch?.Invoke();
    private void OnMoveCTX(InputAction.CallbackContext ctx) => Move = ctx.ReadValue<Vector2>();
    private void OnMoveCTXCancel(InputAction.CallbackContext ctx) => Move = Vector2.zero;
    private void OnLookCTX(InputAction.CallbackContext ctx) => Look = ctx.ReadValue<Vector2>();
    private void OnLookCTXCancel(InputAction.CallbackContext ctx) => Look = Vector2.zero;
    private void OnRunCTX(InputAction.CallbackContext ctx) => Run = true;
    private void OnRunCTXCancel(InputAction.CallbackContext ctx) => Run = false;
    private void OnAttackCTX(InputAction.CallbackContext ctx) => Attack = true;
    private void OnAttackCTXCancel(InputAction.CallbackContext ctx) => Attack = false;
    private void OnInteractCTX(InputAction.CallbackContext ctx) => Interact = true;
    private void OnInteractCTXCancel(InputAction.CallbackContext ctx) => Interact = false;
}

[System.Serializable]
public struct InputNames
{
    [field: SerializeField] public string MoveActionName { get; private set; }
    [field: SerializeField] public string LookActionName { get; private set; }
    [field: SerializeField] public string AttackActionName { get; private set; }
    [field: SerializeField] public string RunActionName { get; private set; }
    [field: SerializeField] public string InteractActionName { get; private set; }
    [field: SerializeField] public string NextActionName { get; private set; }
}

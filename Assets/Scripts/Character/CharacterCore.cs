using UnityEngine;
using Zenject;

public class CharacterCore : MonoBehaviour
{
   private SceneCamera _sceneCamera;
   private CameraTargetTag _cameraTargetTag;
   private AIInput _aiInput;
   private CharacterInputHandler _characterInputHandler;
   private CharacterMovementHandler _characterMovementHandler;
   private Transform _characterTransform;

   private SceneCharacterContainer _container;

   [Inject]
   private void Construct(
      Animator animator,
      SceneCamera sceneCamera,
      CameraTargetTag cameraTargetTag,
      PlayerInput playerInput,
      AIInput aiInput,
      SceneCharacterContainer container,
      IDamageable damageable
      )
   {
      _sceneCamera = sceneCamera;
      _cameraTargetTag = cameraTargetTag;
      _aiInput = aiInput;
      _characterInputHandler = new CharacterInputHandler(playerInput, _aiInput); 
      _characterMovementHandler = new CharacterMovementHandler(animator, _characterInputHandler, transform, damageable);
      _container = container;
      _container.RegisterCharacter(this);
   }

   public void SelectByPlayer()
   {
      _sceneCamera.SetTarget(_cameraTargetTag.transform);
      
      _characterInputHandler.SetupInput(InputSourceMode.Player);
      
      _aiInput.Disable();
   }

   public void ResetPlayerSelection()
   {
      _sceneCamera.SetTarget(null);
      
      _characterInputHandler.SetupInput(InputSourceMode.AI);
      
      _aiInput.Enable();
   }

   private void Update()
   { 
      _characterInputHandler.UpdateInput();
      _characterMovementHandler.UpdateMovement();
   }

   private void OnDestroy()
   {
      _container.UnregisterCharacter(this);
      _characterMovementHandler.OnDestroy();
   }
}
using UnityEngine;
using Zenject;

public class CharacterCore : MonoBehaviour
{
   private SceneCamera _sceneCamera;
   private CameraTargetTag _cameraTargetTag;
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
      SceneCharacterContainer container
      )
   {
      _sceneCamera = sceneCamera;
      _cameraTargetTag = cameraTargetTag;
      _characterInputHandler = new CharacterInputHandler(playerInput, aiInput); 
      _characterMovementHandler = new CharacterMovementHandler(animator, _characterInputHandler, transform);
      _container = container;
      _container.RegisterCharacter(this);
   }

   public void SelectByPlayer()
   {
      _sceneCamera.SetTarget(_cameraTargetTag.transform);
      
      _characterInputHandler.SetupInput(InputSourceMode.Player);
   }

   public void ResetPlayerSelection()
   {
      _sceneCamera.SetTarget(null);
      
      _characterInputHandler.SetupInput(InputSourceMode.AI);
   }

   private void Update()
   { 
      _characterInputHandler.UpdateInput();
      _characterMovementHandler.UpdateMovement();
   }

   private void OnDestroy()
   {
      _container.UnregisterCharacter(this);
   }
}
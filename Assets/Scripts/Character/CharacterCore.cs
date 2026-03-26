using UnityEngine;
using Zenject;

public class CharacterCore : MonoBehaviour
{
   private Animator _animator;
   private SceneCamera _sceneCamera;
   private CameraTargetTag _cameraTargetTag;
   private CharacterInputHandler _characterInputHandler;
   private Transform _characterTransform;

   private int _moveYHash;//
   private int _moveXHash;//
   private int _blendIndexHash;// перенести

   [Inject]
   private void Construct(
      Animator animator,
      SceneCamera sceneCamera,
      CameraTargetTag cameraTargetTag,
      PlayerInput playerInput
      )
   {
      _animator = animator;
      _sceneCamera = sceneCamera;
      _cameraTargetTag = cameraTargetTag;
      _characterInputHandler = new CharacterInputHandler(playerInput, null);
   }

   private void Start()
   {
      _sceneCamera.SetTarget(_cameraTargetTag.transform);
      
      _characterTransform = transform;
      
      CreateHash();
      
      _characterInputHandler.SetupInput(InputSourceMode.Player);
   }

   private void CreateHash()
   {
      _moveXHash = Animator.StringToHash("MoveX");
      _moveYHash = Animator.StringToHash("MoveY");
      _blendIndexHash = Animator.StringToHash("BlendIndex");
   }

   private void Update()
   {
      _animator.SetFloat(_moveXHash, _characterInputHandler.MoveInput.x);
      _animator.SetFloat(_moveYHash, _characterInputHandler.MoveInput.y);
      _animator.SetInteger(_blendIndexHash, Mathf.Abs(_characterInputHandler.MoveInput.y) > 0 ? 1 : 0);
      
      if (_characterInputHandler.Rotation != Vector3.zero)
      {
         Rotation(_characterTransform, _characterInputHandler.Rotation, 120);
      }
   }
   
   private void Rotation(Transform character, Vector3 rotation, float speed)
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

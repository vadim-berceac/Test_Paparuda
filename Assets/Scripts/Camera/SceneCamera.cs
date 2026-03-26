using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class SceneCamera : MonoBehaviour
{
   [field: SerializeField] public Transform MainCamera { get; set; }
   [field: SerializeField] public CharacterSelectorCameraSettings CameraSettings { get; set; }
   
   private Transform _target;
   private ICameraInput _cameraInput;
   private const float Threshold = 0.01f;
   private float _targetYaw;
   private float _targetPitch;

   [Inject]
   private void Construct(PlayerInput playerInput)
   {
      _cameraInput = playerInput;
   }

   public void SetTarget(Transform target)
   {
      _target = target;
      CameraSettings.VirtualCamera.Target.TrackingTarget = target;
   }

   private void LateUpdate()
   {
      CameraRotation();
   }
   
   private void CameraRotation()
   {
      if (_target == null)
      {
         return;
      }

      if (_cameraInput.Look.sqrMagnitude >= Threshold)
      {
         _targetYaw += _cameraInput.Look.x * CameraSettings.HorizontalRotationSpeed;
         _targetPitch += _cameraInput.Look.y * CameraSettings.VerticalRotationSpeed;
      }

      _targetYaw = ClampAngle(_targetYaw, float.MinValue, float.MaxValue);
      _targetPitch = ClampAngle(_targetPitch, CameraSettings.BottomClamp, CameraSettings.TopClamp);

      _target.rotation = Quaternion.Euler(
         _targetPitch + CameraSettings.CameraAngleOverride,
         _targetYaw,
         0.0f
      );
   }
   
   private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
   {
      if (lfAngle < -360f) lfAngle += 360f;
      if (lfAngle > 360f) lfAngle -= 360f;
      return Mathf.Clamp(lfAngle, lfMin, lfMax);
   }
}

[System.Serializable]
public struct CharacterSelectorCameraSettings
{
   [field: SerializeField] public CinemachineCamera VirtualCamera { get; set; }
   [field: SerializeField, Range(0, 1)] public float HorizontalRotationSpeed { get; set; }
   [field: SerializeField, Range(0, 1)] public float VerticalRotationSpeed { get; set; }
   [field: SerializeField, Range(0, 90)] public float TopClamp { get; set; }
   [field: SerializeField, Range(-90, 0)] public float BottomClamp { get; set; }
   [field: SerializeField] public float CameraAngleOverride { get; set; }
}
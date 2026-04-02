namespace Scripts.Camera
{
    using Unity.Cinemachine;
    using UnityEngine;

    public class VirtualCamera : MonoBehaviour
    {
        [SerializeField]
        private CinemachineCamera cinemachineCamera;
        
        public void LookAt(Transform target)
        {
            cinemachineCamera.Follow = cinemachineCamera.LookAt = target;
        }
        
        public void ResetLook()
        {
            cinemachineCamera.Follow = cinemachineCamera.LookAt = null;
        }

        public Vector3 GetTransformDirection(Vector3 direction)
        {
            var newDirection = cinemachineCamera.transform.TransformDirection(direction);
            newDirection.y = 0f;
            return newDirection;
        }
    }
}
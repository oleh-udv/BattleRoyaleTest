namespace Scripts.Units.Movement
{
    using UnityEngine;

    [RequireComponent(typeof(CharacterController))]
    public class UnitMovement : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private float speed;
        [SerializeField] private float smoothness = 0.1f;

        [Header("References")] 
        [SerializeField] private CharacterController characterController;

        private Vector3 direction;
        private Vector3 targetVelocity;
        
        private Vector3 smoothedVelocity;
        private Vector3 currentVelocity;

        private void OnValidate() =>
            characterController = GetComponent<CharacterController>();

        public void SetMovementDirection(Vector3 direction) =>
            this.direction = direction;

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            targetVelocity = direction * speed;

            smoothedVelocity =
                Vector3.SmoothDamp(smoothedVelocity, targetVelocity,
                    ref currentVelocity, smoothness);

            characterController.Move(smoothedVelocity * Time.deltaTime);
        }
    }
}
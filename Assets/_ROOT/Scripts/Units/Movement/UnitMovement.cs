namespace Scripts.Units.Movement
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UIElements;

    [RequireComponent(typeof(CharacterController))]
    public class UnitMovement : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private float speed;
        [SerializeField] private float smoothness = 0.1f;
        [SerializeField] private float reachedDestination = 0.1f;
        [SerializeField] private float movePointUpdateTime = 0.1f;
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float lift = 0.001f;
        
        [Header("References")] 
        [SerializeField] private CharacterController characterController;

        private Vector3 direction;
        private Vector3 targetVelocity;
        
        private Vector3 smoothedVelocity;
        private Vector3 currentVelocity;

        private Coroutine moveToPoint;

        private void OnValidate() =>
            characterController = GetComponent<CharacterController>();

        public Vector3 Direction => direction;

        public void SetMovementDirection(Vector3 direction) =>
            this.direction = direction;

        private void FixedUpdate()
        {
            Move();
            Rotate();
        }

        public void SetMovePoint(Vector3 point)
        {
            moveToPoint = StartCoroutine(MoveToPoint(point));
        }
        
        public void StopMoveToPoint()
        {
            if(moveToPoint != null)
                StopCoroutine(moveToPoint);
            
            SetMovementDirection(Vector3.zero);
        }

        private IEnumerator MoveToPoint(Vector3 point)
        {
            var data = CalculatePointData(point);
            var tickTime = new WaitForSeconds(movePointUpdateTime);
            
            while (!data.ReachedDestination)
            {
                data = CalculatePointData(point);
                SetMovementDirection(data.Direction);
                yield return tickTime;
            }

            SetMovementDirection(Vector3.zero);
        }
        
        private MoveToPointData CalculatePointData(Vector3 point)
        {
            var difference = point - transform.position;
            var sqrMagnitude = difference.sqrMagnitude;

            return new MoveToPointData()
            {
                Direction = difference.normalized,
                ReachedDestination = sqrMagnitude <= reachedDestination
            };
        }

        private void Move()
        {
            if (direction.sqrMagnitude < lift)
                return;
            
            targetVelocity = direction * speed;

            smoothedVelocity =
                Vector3.SmoothDamp(smoothedVelocity, targetVelocity,
                    ref currentVelocity, smoothness);

            characterController.Move(smoothedVelocity * Time.deltaTime);
        }
        
        private void Rotate()
        {
            if (direction.sqrMagnitude < lift)
                return;
            
            var targetRotation = Quaternion.LookRotation(direction);

            transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        public struct MoveToPointData
        {
            public Vector3 Direction;
            public bool ReachedDestination;
        }
    }
}
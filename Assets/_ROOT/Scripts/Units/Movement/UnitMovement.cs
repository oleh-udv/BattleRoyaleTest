namespace Scripts.Units.Movement
{
    using System.Collections;
    using UnityEngine;

    [RequireComponent(typeof(CharacterController))]
    public class UnitMovement : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private float speed;
        [SerializeField] private float smoothness = 0.1f;
        [SerializeField] private float reachedDestination = 0.1f;
        [SerializeField] private float movePointUpdateTime = 0.1f;
        
        [Header("References")] 
        [SerializeField] private CharacterController characterController;

        private Vector3 direction;
        private Vector3 targetVelocity;
        
        private Vector3 smoothedVelocity;
        private Vector3 currentVelocity;

        private Coroutine moveToPoint;

        private void OnValidate() =>
            characterController = GetComponent<CharacterController>();

        public void SetMovementDirection(Vector3 direction) =>
            this.direction = direction;

        private void FixedUpdate()
        {
            Move();
        }

        public void SetMovePoint(Vector3 point)
        {
            moveToPoint = StartCoroutine(MoveToPoint(point));
        }
        
        public void StopMoveToPoint()
        {
            if(moveToPoint != null)
                StopCoroutine(moveToPoint);
        }

        private IEnumerator MoveToPoint(Vector3 point)
        {
            var data = CalculateData();
            var tickTime = new WaitForSeconds(movePointUpdateTime);
            
            while (!data.ReachedDestination)
            {
                data = CalculateData();
                SetMovementDirection(data.Direction);
                yield return tickTime;
            }

            SetMovementDirection(Vector3.zero);

            MoveToPointData CalculateData()
            {
                var difference = point - transform.position;
                var sqrMagnitude = difference.sqrMagnitude;

                return new MoveToPointData()
                {
                    Direction = difference.normalized,
                    ReachedDestination = sqrMagnitude <= reachedDestination
                };
            }
        }

        private void Move()
        {
            targetVelocity = direction * speed;

            smoothedVelocity =
                Vector3.SmoothDamp(smoothedVelocity, targetVelocity,
                    ref currentVelocity, smoothness);

            characterController.Move(smoothedVelocity * Time.deltaTime);
        }
        
        public struct MoveToPointData
        {
            public Vector3 Direction;
            public bool ReachedDestination;
        }
    }
}
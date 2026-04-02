namespace Scripts.Units.Movement
{
    using Camera;
    using Infrastructure.Input;
    using UnityEngine;
    using Zenject;

    [RequireComponent(typeof(UnitMovement))]
    public class PlayerMovementControl : MonoBehaviour
    {
        [Inject] 
        private IInputProvider InputProvider { get; set; }
        [Inject]
        private VirtualCamera VirtualCamera { get; set; }

        [SerializeField] 
        private UnitMovement unitMovement;
        
        private void OnValidate()
        {
            unitMovement = GetComponent<UnitMovement>();
        }

        private void Start()
        {
            VirtualCamera.LookAt(transform);
        }

        private void Update()
        {
            var input = InputProvider.Axis;

            var movementVector = VirtualCamera.GetTransformDirection(input);
            movementVector.Normalize();
            
            unitMovement.SetMovementDirection(movementVector);
        }
    }
}
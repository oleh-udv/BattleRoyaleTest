namespace Scripts.Units.Movement
{
    using Infrastructure.Input;
    using UnityEngine;
    using Zenject;

    [RequireComponent(typeof(UnitMovement))]
    public class PlayerMovementControl : MonoBehaviour
    {
        [Inject] 
        private IInputProvider InputProvider { get; set; }

        [SerializeField] 
        private UnitMovement unitMovement;
        
        private Camera mainCamera;

        private void OnValidate()
        {
            unitMovement = GetComponent<UnitMovement>();
        }

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            var input = InputProvider.Axis;

            var movementVector = mainCamera.transform.TransformDirection(input);
            movementVector.Normalize();
            
            unitMovement.SetMovementDirection(movementVector);
        }
    }
}
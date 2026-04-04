namespace Scripts.Units.Animations
{
    using Movement;
    using UnityEngine;
    using Zenject;

    public class PlayerAnimation : MonoBehaviour
    {
        [Inject]
        private UnitAnimationConstants animationConstants;
        
        [SerializeField] private Animator animator;
        [SerializeField] private UnitMovement unitMovement;
        
        [Header("Settings")] 
        [SerializeField] private float runVelocityLift = 0.05f;
        
        private void Update()
        {
            var velocityMagnitude = unitMovement.Direction.normalized.sqrMagnitude;
            SetState(runVelocityLift < velocityMagnitude ? AnimationStates.Run : AnimationStates.Idle);
        }
        
        public void SetState(AnimationStates state)
        {
            switch (state)
            {
                case AnimationStates.Idle:
                    animator.SetTrigger(animationConstants.IdleTrigger);
                    animator.ResetTrigger(animationConstants.RunTrigger);
                    break;
                case AnimationStates.Run:
                    animator.SetTrigger(animationConstants.RunTrigger);
                    animator.ResetTrigger(animationConstants.IdleTrigger);
                    break;
            }
        }
    }
}
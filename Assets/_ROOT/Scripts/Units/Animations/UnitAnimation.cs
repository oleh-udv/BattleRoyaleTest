namespace Scripts.Units.Animations
{
    using System;
    using Movement;
    using UnityEngine;

    public class UnitAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private UnitMovement unitMovement;

        [Header("Settings")] 
        [SerializeField] private float runVelocityLift = 0.05f;
        
        private const string RunTrigger = "Run";
        private const string IdleTrigger = "Idle";
        
        private bool isAlive = true;
        private bool isСalmly = true;

        private void Update()
        {
            if (isAlive && isСalmly)
            {
                var velocityMagnitude = unitMovement.Direction.normalized.sqrMagnitude;
                SetState(runVelocityLift < velocityMagnitude ? AnimationStates.Run : AnimationStates.Idle);
            }
        }

        public void SetState(AnimationStates state)
        {
            switch (state)
            {
                case AnimationStates.Idle:
                    animator.ResetTrigger(RunTrigger);
                    animator.SetTrigger(IdleTrigger);
                    break;
                case AnimationStates.Run:
                    animator.ResetTrigger(IdleTrigger);
                    animator.SetTrigger(RunTrigger);
                    break;
            }
        }
    }
}
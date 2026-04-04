namespace Scripts.Units.Animations
{
    using System;
    using System.Collections.Generic;
    using Movement;
    using Player;
    using UnityEngine;
    using Zenject;

    public class PlayerAnimation : MonoBehaviour
    {
        [Inject]
        private UnitAnimationConstants animationConstants;

        [SerializeField] private Player player;
        [SerializeField] private Transform view;
        [SerializeField] private Animator animator;
        [SerializeField] private UnitMovement unitMovement;
        
        [Header("Settings")] 
        [SerializeField] private float runVelocityLift = 0.05f;
        
        [Header("LevelUp")] 
        [SerializeField] private List<float> scaleByLevel;
        
        private void Start()
        {
            player.OnLevelUp += LevelUpAnimation;
        }

        private void OnDestroy()
        {
            player.OnLevelUp -= LevelUpAnimation;
        }

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

        private void LevelUpAnimation()
        {
            var level = player.Level;
            view.localScale = Vector3.one * scaleByLevel[level];
        }
    }
}
namespace Scripts.Units.Animations
{
    using System;
    using DG.Tweening;
    using Movement;
    using UnityEngine;
    using Zenject;

    public class UnitAnimation : MonoBehaviour
    {
        [Inject]
        private UnitAnimationConstants animationConstants;

        [SerializeField] private Animator animator;
        [SerializeField] private UnitMovement unitMovement;
        [SerializeField] private Unit unit;

        [Header("Settings")] 
        [SerializeField] private float runVelocityLift = 0.05f;
        [SerializeField] private float waitDeathTime = 1f;
        
        private Sequence deathSequence;
        private bool isСalmly = true;

        private void Start()
        {
            unit.OnDied += UnitDead;
            unit.OnShoot += Attack;
            unit.OnEndBattle += ReleaseUnit;
        }

        private void OnDestroy()
        {
            unit.OnDied -= UnitDead;
            unit.OnShoot -= Attack;
            unit.OnEndBattle -= ReleaseUnit;
            
            deathSequence.Kill();
        }

        private void Update()
        {
            if (isСalmly)
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
                    animator.SetTrigger(animationConstants.IdleTrigger);
                    animator.ResetTrigger(animationConstants.RunTrigger);
                    animator.ResetTrigger(animationConstants.ShootTrigger);
                    animator.ResetTrigger(animationConstants.DeathTrigger);
                    break;
                case AnimationStates.Run:
                    animator.SetTrigger(animationConstants.RunTrigger);
                    animator.ResetTrigger(animationConstants.IdleTrigger);
                    animator.ResetTrigger(animationConstants.ShootTrigger);
                    animator.ResetTrigger(animationConstants.DeathTrigger);
                    break;
                case AnimationStates.Shoot:
                    animator.SetTrigger(animationConstants.ShootTrigger);
                    animator.ResetTrigger(animationConstants.IdleTrigger);
                    animator.ResetTrigger(animationConstants.DeathTrigger);
                    animator.ResetTrigger(animationConstants.RunTrigger);
                    break;
                case AnimationStates.Death:
                    animator.SetTrigger(animationConstants.DeathTrigger);
                    animator.ResetTrigger(animationConstants.IdleTrigger);
                    animator.ResetTrigger(animationConstants.ShootTrigger);
                    animator.ResetTrigger(animationConstants.RunTrigger);
                    break;
            }
        }
        
        private void Attack()
        {
            isСalmly = false;
            SetState(AnimationStates.Shoot);
        }

        private void ReleaseUnit()
        {
            isСalmly = true;
        }

        private void UnitDead()
        {
            SetState(AnimationStates.Death);

            deathSequence = DOTween.Sequence();
            deathSequence.AppendInterval(waitDeathTime);
            deathSequence.OnComplete(() => unit.gameObject.SetActive(false));
        }
    }
}
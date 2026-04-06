namespace Scripts.Units.Animations
{
    using System;
    using System.Collections.Generic;
    using DG.Tweening;
    using Movement;
    using UnityEngine;
    using Zenject;

    public class UnitAnimation : MonoBehaviour
    {
        [Inject]
        private UnitAnimationConstants animationConstants;

        [SerializeField] private Animator animator;
        [SerializeField] private Transform view;
        [SerializeField] private UnitMovement unitMovement;
        [SerializeField] private Unit unit;

        [Header("Settings")] 
        [SerializeField] private float runVelocityLift = 0.05f;
        [SerializeField] private float waitDeathTime = 1f;
        [SerializeField] private float appearanceTime = 0.5f;

        [Header("LevelUp")] 
        [SerializeField] private List<float> scaleByLevel;

        private Sequence appearanceSequence;
        private Sequence deathSequence;
        private bool isСalmly = true;

        private void Awake()
        {
            unit.OnSetup += PlayAppearance;
            unit.OnDied += UnitDead;
            unit.OnShoot += Attack;
            unit.OnEndBattle += ReleaseUnit;
            unit.OnSetup += PlayAppearance;
        }

        private void OnDisable()
        {
            appearanceSequence?.Kill();
            deathSequence?.Kill();
        }

        private void OnDestroy()
        {
            unit.OnDied -= UnitDead;
            unit.OnShoot -= Attack;
            unit.OnEndBattle -= ReleaseUnit;
            unit.OnSetup -= PlayAppearance;
        }

        private void Update()
        {
            if (isСalmly)
            {
                var velocityMagnitude = unitMovement.Direction.normalized.sqrMagnitude;
                SetState(runVelocityLift < velocityMagnitude ? AnimationStates.Run : AnimationStates.Idle);
            }
        }

        private void PlayAppearance()
        {
            var level = unit.Level;
            
            appearanceSequence?.Kill();
            view.localScale = Vector3.zero;
            appearanceSequence = DOTween.Sequence();

            appearanceSequence.Append(view.DOScale(Vector3.one * scaleByLevel[level], appearanceTime));
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
namespace Scripts.Zones
{
    using System;
    using System.Collections.Generic;
    using DG.Tweening;
    using Units.Player;
    using UnityEngine;

    public class ArmyFormationZone : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private List<Transform> unitPoints;
        [SerializeField] private int startPointsCount;
        [SerializeField] private int upgradePointsCount;
        
        [Header("View")]
        [SerializeField] private Transform view;
        [SerializeField] private ParticleSystem upgradeParticle;
        [SerializeField] private float scaleTime = 0.1f;

        private List<Transform> activePoints;
        private Tween scaleTween;
        private int freePointIndex;

        public event Action OnUpgrade;

        public event Action OnFull;

        private void Start()
        {
            ActivatePoints(startPointsCount);
            player.OnLevelUp += Upgrade;
        }

        private void OnDestroy()
        {
            player.OnLevelUp -= Upgrade;
        }

        public Vector3 GetFreePoint()
        {
            if(freePointIndex >= activePoints.Count)
                return Vector3.zero;
            
            var point = activePoints[freePointIndex];
            freePointIndex++;
            if (freePointIndex >= activePoints.Count)
                OnFull?.Invoke();

            return point.position;
        }

        public void ClearPoints()
        {
            freePointIndex = 0;
        }

        private void Upgrade()
        {
            ActivatePoints(upgradePointsCount);
            
            scaleTween?.Kill();
            scaleTween = view.DOScaleZ(2f, scaleTime);
            upgradeParticle.Play();
            
            OnUpgrade?.Invoke();
        }

        private void ActivatePoints(int count)
        {
            activePoints = new List<Transform>(count);

            for (int i = 0; i < count; i++)
            {
                activePoints.Add(unitPoints[i]);
                unitPoints[i].gameObject.SetActive(true);
            }
        }
    }
}
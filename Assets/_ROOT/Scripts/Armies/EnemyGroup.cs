namespace Scripts.Armies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DG.Tweening;
    using Units;
    using Units.EnemyArmy;
    using Units.PlayerArmy;
    using Units.Settings;
    using UnityEngine;
    using Zenject;

    public class EnemyGroup : MonoBehaviour
    {
        [Inject] 
        private UnitsFactory UnitsFactory { get; set; }

        [Header("Vision")]
        [SerializeField] private Collider visionCollider;
        
        [Header("Settings")]
        [SerializeField] private UnitLevelingSettings unitLevelingSettings;
        [SerializeField] private EnemyArmyUnit enemyPrefab;
        [SerializeField] private int unitsLevel;

        [Header("Units")] 
        [SerializeField] private Transform unitsContainer;
        [SerializeField] private List<Transform> unitPoints;

        [Header("View")] 
        [SerializeField] private Transform view;
        [SerializeField] private float scaleTime = 0.5f;

        private List<EnemyArmyUnit> units = new();
        private Tween scaleTween;
        
        public event Action OnGroupLose;
        public bool IsAlive => units.Any(u => u.IsAlive);

        private void Start()
        {
            var levelSettings = unitLevelingSettings.GetSettingsByLevel(unitsLevel);
                
            foreach (var point in unitPoints)
            {
                var unit = (EnemyArmyUnit)UnitsFactory.Create(enemyPrefab, unitsContainer);
                units.Add(unit);
                
                unit.transform.position = point.position;
                unit.transform.rotation = point.rotation;
                
                unit.OnDied += CheckOnLose;
                unit.Setup(levelSettings);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerArmyUnit playerArmyUnit))
                DetectPlayerArmyUnit(playerArmyUnit);
        }

        private void OnDestroy()
        {
            units.ForEach(u => u.OnDied -= CheckOnLose);
        }

        public void SetWaitUnits(bool isWait)
        {
            visionCollider.enabled = isWait;
        }

        public void ReturnUnits()
        {
            foreach (var unit in units)
            {
                if (unit.IsAlive)
                {
                    unit.ReturnToStartPoint();
                    unit.SetReadyToFight(false);
                }
            }
        }

        private void CheckOnLose()
        {
            if (!IsAlive)
            {
                scaleTween?.Kill();
                scaleTween = view.DOScale(Vector3.zero, scaleTime);
                
                OnGroupLose?.Invoke();
            }
        }

        private void DetectPlayerArmyUnit(PlayerArmyUnit playerArmyUnit)
        {
            foreach (var unit in units)
            {
                if(!unit.IsAlive)
                    continue;
                
                unit.UnitMovement.SetMovePoint(playerArmyUnit.transform.position);
                unit.SetReadyToFight(true);
            }

            SetWaitUnits(false);
        }
    }
}
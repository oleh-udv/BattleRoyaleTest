namespace Scripts.Armies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        private List<EnemyArmyUnit> units = new();

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
            if(!IsAlive)
                OnGroupLose?.Invoke();
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
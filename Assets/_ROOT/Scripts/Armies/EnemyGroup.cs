namespace Scripts.Armies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Units.EnemyArmy;
    using Units.PlayerArmy;
    using Units.Settings;
    using UnityEngine;

    public class EnemyGroup : MonoBehaviour
    {
        [Header("Vision")]
        [SerializeField] private Collider visionCollider;
        
        [Header("Settings")]
        [SerializeField] private UnitLevelingSettings unitLevelingSettings;
        [SerializeField] private int unitsLevel;
        
        [Header("Units")]
        [SerializeField] private List<EnemyArmyUnit> units;

        public event Action OnGroupLose;
        public bool IsAlive => units.Any(u => u.IsAlive);

        private void Start()
        {
            var levelSettings = unitLevelingSettings.GetSettingsByLevel(unitsLevel);
                
            foreach (var unit in units)
            {
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
            Debug.Log("ReturnUnits");
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
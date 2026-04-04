namespace Scripts.Armies
{
    using System;
    using System.Collections.Generic;
    using Units;
    using Units.Settings;
    using UnityEngine;
    using Zones;

    public class PlayerArmy : MonoBehaviour
    {
        [SerializeField] private ArmyFormationZone armyFormationZone;
        [SerializeField] private PlayerTrigger startBattleTrigger;
        [SerializeField] private EnemyArmy enemyArmy;

        private List<Unit> playerArmy = new();

        public event Action OnStartProduction;
        public event Action OnStopProduction;

        private void Start()
        {
            StartProduction();
            
            armyFormationZone.OnFull += StopProduction;
            startBattleTrigger.OnPlayerEnter += MoveToBattle;
        }

        private void OnDestroy()
        {
            armyFormationZone.OnFull -= StopProduction;
            startBattleTrigger.OnPlayerEnter -= MoveToBattle;
        }

        private void MoveToBattle()
        {
            if (playerArmy.Count == 0)
                return;
            
            StopProduction();

            foreach (var unit in playerArmy)
            {
                unit.MoveToPoint(enemyArmy.GetAttackPoint());
                unit.SetReadyToFight(true);
            }
        }

        public void EndBattle()
        {
            StartProduction();
        }

        public void AddUnit(Unit unit, LevelingSettings levelingSettings)
        {
            unit.Setup(levelingSettings);
            playerArmy.Add(unit);

            var movePoint = armyFormationZone.GetFreePoint();
            unit.MoveToPoint(movePoint);
        }

        private void StartProduction()
        {
            OnStartProduction?.Invoke();
        }
        
        private void StopProduction()
        {
            OnStopProduction?.Invoke();
        }
    }
}
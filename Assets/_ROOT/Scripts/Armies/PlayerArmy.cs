namespace Scripts.Armies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            startBattleTrigger.OnPlayerEnter += PrepareToBattle;
            enemyArmy.OnGroupLose += ChangeTarget;
        }

        private void OnDestroy()
        {
            armyFormationZone.OnFull -= StopProduction;
            startBattleTrigger.OnPlayerEnter -= PrepareToBattle;
            enemyArmy.OnGroupLose -= ChangeTarget;
            
            EndBattle(false);
        }
        
        public void AddUnit(Unit unit, LevelingSettings levelingSettings)
        {
            unit.Setup(levelingSettings);
            playerArmy.Add(unit);

            var movePoint = armyFormationZone.GetFreePoint();
            unit.UnitMovement.SetMovePoint(movePoint);
        }

        private void PrepareToBattle()
        {
            playerArmy.ForEach(u => u.OnDied += CheckEndBattle);
            
            MoveToBattle();
        }

        private void MoveToBattle()
        {
            if (playerArmy.Count == 0)
                return;
            
            StopProduction();
            enemyArmy.SetAttackedUnits(playerArmy);
            armyFormationZone.ClearPoints();

            foreach (var unit in playerArmy)
            {
                if(!unit.IsAlive)
                    continue;
                
                unit.UnitMovement.SetMovePoint(enemyArmy.GetAttackPoint());
                unit.SetReadyToFight(true);
            }
        }
        
        private void ChangeTarget(bool targetExist)
        {
            if (targetExist)
                MoveToBattle();
            else
                playerArmy.ForEach(u => u.UnitMovement.StopMoveToPoint());
        }

        private void CheckEndBattle()
        {
            if (!playerArmy.Any(u => u.IsAlive))
                EndBattle();
        }

        private void EndBattle(bool startProduction = true)
        {
            foreach (var unit in playerArmy)
            {
                unit.OnDied -= CheckEndBattle;
                unit.SetReadyToFight(false);
            }
            
            playerArmy.Clear();
            
            if(startProduction)
                StartProduction();
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
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

        private List<Unit> playerArmy = new();

        public event Action OnStartProduction;
        public event Action OnStopProduction;

        private void Start()
        {
            StartProduction();
            armyFormationZone.OnFull += StopProduction;
        }

        private void OnDestroy()
        {
            armyFormationZone.OnFull -= StopProduction;
        }

        public void MoveToButtle()
        {
            StopProduction();
        }

        public void EndButtle()
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
namespace Scripts.Garrisons
{
    using System.Collections.Generic;
    using Armies;
    using Units;
    using Units.PlayerArmy;
    using UnityEngine;
    using Zenject;

    public class GarrisonsConductor : MonoBehaviour
    {
        [Inject] 
        private UnitsFactory UnitsFactory { get; set; }
        
        [SerializeField] private PlayerArmy playerArmy;
        
        [Space]
        [SerializeField] private List<Garrison> garrisons;
        [SerializeField] private Unit unitPrefab;
        [SerializeField] private int unitsPoolCount;
        
        private PlayerArmyPool playerArmyPool;

        private void Start()
        {
            playerArmyPool = new PlayerArmyPool(unitPrefab, playerArmy.transform, unitsPoolCount, UnitsFactory);

            playerArmy.OnStartProduction += StartProduction;
            playerArmy.OnStopProduction += StopProduction;
            garrisons.ForEach(g => g.OnSpawnTimerEnd += SpawnUnit);
        }
        
        private void OnDestroy()
        {
            StopProduction();
            playerArmy.OnStartProduction -= StartProduction;
            playerArmy.OnStopProduction -= StopProduction;
            garrisons.ForEach(g => g.OnSpawnTimerEnd -= SpawnUnit);
        }

        private void StartProduction()
        {
            garrisons.ForEach(g => g.StartSpawnTimer());
        }

        private void StopProduction()
        {
            garrisons.ForEach(g => g.StopSpawnTimer());
        }

        private void SpawnUnit(Garrison garrison)
        {
            var unit = playerArmyPool.GetFreeElement();
            unit.transform.position = garrison.transform.position;
            playerArmy.AddUnit(unit, garrison.UnitLevelingSettings);
        }
    }
}
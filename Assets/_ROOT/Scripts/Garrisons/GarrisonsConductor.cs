namespace Scripts.Garrisons
{
    using System.Collections.Generic;
    using PlayerArmy;
    using Units;
    using Units.Settings;
    using UnityEngine;
    using Zenject;

    public class GarrisonsConductor : MonoBehaviour
    {
        [Inject] 
        private PlayerUnitsFactory PlayerUnitsFactory { get; set; }
        
        [SerializeField] private List<Garrison> garrisons;
        [SerializeField] private Unit unitPrefab;
        [SerializeField] private Transform unitsParent;
        [SerializeField] private int unitsPoolCount;
        
        private PlayerArmyPool playerArmyPool;

        private void Start()
        {
            playerArmyPool = new PlayerArmyPool(unitPrefab, unitsParent, unitsPoolCount, PlayerUnitsFactory);
            garrisons.ForEach(g => g.OnSpawnTimerEnd += SpawnUnit);
        }
        
        private void OnDestroy()
        {
            garrisons.ForEach(g => g.OnSpawnTimerEnd -= SpawnUnit);
        }

        private void SpawnUnit(Garrison garrison)
        {
            var unit = playerArmyPool.GetFreeElement();
            unit.Setup(garrison.UnitLevelingSettings);
        }
    }
}
namespace Scripts.Garrisons
{
    using System;
    using System.Collections;
    using Units.Settings;
    using UnityEngine;

    public class Garrison : MonoBehaviour
    {
        [Header("Units")] 
        [SerializeField] private UnitLevelingSettings unitsLevelingSettings;
        
        [Header("Timer")] 
        [SerializeField] private float spawnTimerTime;
        
        public LevelingSettings UnitLevelingSettings 
            => unitsLevelingSettings.GetSettingsByLevel(currentLevel);
        
        private int currentLevel;
        private bool spawnTimerActive;
        private Coroutine spawnTimer;

        public int Level => currentLevel;
        
        public event Action OnActivate;
        public event Action OnDeactivate;
        public event Action<Garrison> OnSpawnTimerEnd;

        private void Start()
        {
            OnActivate?.Invoke();
        }

        private void OnDestroy()
        {
            StopSpawnTimer();
        }
        
        public void StartSpawnTimer()
        {
            if (spawnTimerActive)
                return;
            
            spawnTimerActive = true;
            spawnTimer = StartCoroutine(SpawnTimer());
        }

        public void StopSpawnTimer()
        {
            spawnTimerActive = false;
            
            if(spawnTimer != null)
                StopCoroutine(spawnTimer);
        }
        
        public void LevelUp()
        {
            currentLevel++;
        }

        private IEnumerator SpawnTimer()
        {
            var spawnTime = new WaitForSeconds(spawnTimerTime);
            
            while (spawnTimerActive)
            {
                yield return spawnTime;
                OnSpawnTimerEnd?.Invoke(this);
            }
        }
    }
}
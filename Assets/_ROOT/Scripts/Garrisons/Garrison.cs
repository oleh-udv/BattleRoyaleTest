namespace Scripts.Garrisons
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Units.Settings;
    using UnityEngine;
    using Zones;

    public class Garrison : MonoBehaviour
    {
        [Header("Units")] 
        [SerializeField] private UnitLevelingSettings unitsLevelingSettings;
        
        [Header("Timer")] 
        [SerializeField] private float spawnTimerTime;
        
        [Header("Buy")]
        [SerializeField] private BuyZone buyZone;
        [SerializeField] private BuyZone levelUpBuyZone;
        [SerializeField] private bool isBought;

        public LevelingSettings UnitLevelingSettings 
            => unitsLevelingSettings.GetSettingsByLevel(currentLevel);
        
        private int currentLevel;
        private bool spawnTimerActive;
        private Coroutine spawnTimer;
        public event Action OnActivate;
        public event Action OnDeactivate;
        public event Action<Garrison> OnSpawnTimerEnd;

        private void Start()
        {
            CheckActive();
            levelUpBuyZone.OnBought += LevelUp; 
        }

        private void OnDestroy()
        {
            buyZone.OnBought -= Activate;
            levelUpBuyZone.OnBought -= LevelUp; 
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

        private void CheckActive()
        {
            if (!isBought)
            {
                buyZone.OnBought += Activate;
                Deactivate();
            }
            else
            {
                Activate();
            }
        }
        
        private void Activate()
        {
            buyZone.OnBought -= Activate;
            StartSpawnTimer();
            levelUpBuyZone.gameObject.SetActive(true);
            OnActivate?.Invoke();
        }

        private void Deactivate()
        {
            StopSpawnTimer();
            OnDeactivate?.Invoke();
        }

        private void LevelUp()
        {
            levelUpBuyZone.gameObject.SetActive(false);
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
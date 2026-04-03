namespace Scripts.Garrisons
{
    using System;
    using UnityEngine;
    using Zones;

    public class Garrison : MonoBehaviour
    {
        [Header("References")]
        [Header("Buy")]
        [SerializeField] private BuyZone buyZone;
        [SerializeField] private bool isBought;

        public event Action OnActivate;
        public event Action OnDeactivate;

        private void Start()
        {
            CheckActive();
        }

        private void OnDestroy()
        {
            buyZone.OnBought -= Activate;
        }

        private void CheckActive()
        {
            if (!isBought)
            {
                buyZone.Setup();
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
            Debug.Log("Activate");
            buyZone.OnBought -= Activate;
            OnActivate?.Invoke();
        }

        private void Deactivate()
        {
            Debug.Log("Deactivate");
            OnDeactivate?.Invoke();
        }

        private void StartSpawnUnits()
        {
            
        }

        private void StopSpawnUnits()
        {
            
        }
    }
}
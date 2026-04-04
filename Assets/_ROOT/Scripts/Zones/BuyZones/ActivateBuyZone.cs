namespace Scripts.Zones.BuyZones
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ActivateBuyZone : BuyZoneBase
    {
        [Header("Price")] 
        [SerializeField] private int price;
        
        [Space] 
        [SerializeField] private List<GameObject> activatedObjects;

        private void Awake()
        {
            activatedObjects.ForEach(obj => obj.SetActive(false));
        }

        protected override void LoadRemainingAmount()
        {
            remainingAmount = price;
            startPrice = price;
        }

        protected override void Buy()
        {
            base.Buy();
            activatedObjects.ForEach(obj => obj.SetActive(true));
            Deactivate();
        }
    }
}
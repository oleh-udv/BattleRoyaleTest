namespace Scripts.Zones.BuyZones
{
    using System.Collections.Generic;
    using Garrisons;
    using UnityEngine;

    public class GarrisonBuyZone : BuyZoneBase
    {
        [SerializeField] private Garrison garrison;
        [SerializeField] private List<int> prices;
        
        private bool IsMaxLevel => garrison.Level >= prices.Count;

        protected override void LoadRemainingAmount()
        {
            if (IsMaxLevel)
            {
                Deactivate();
                return;
            }

            startPrice = prices[garrison.Level];
            remainingAmount = prices[garrison.Level];
            
            buyProgress.text = (startPrice - remainingAmount) + "/" + startPrice;
        }

        protected override void Buy()
        {
            base.Buy();
            garrison.LevelUp();

            LoadRemainingAmount();

            if (!isActiveAndEnabled)
                return;
            
            isBought = false;
            StartProgress();
        }
    }
}
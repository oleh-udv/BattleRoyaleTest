namespace Scripts.Zones.BuyZones
{
    using System.Collections.Generic;
    using Units.Player;
    using UnityEngine;

    public class PlayerBuyZone : BuyZoneBase
    {
        [SerializeField] private Player player;
        [SerializeField] private List<int> prices;
        
        private bool IsMaxLevel => player.Level >= prices.Count - 1;
        
        protected override void LoadRemainingAmount()
        {
            if(IsMaxLevel)
                Deactivate();

            startPrice = prices[player.Level];
            remainingAmount = prices[player.Level];
        }

        protected override void Buy()
        {
            base.Buy();
            player.LevelUp();

            LoadRemainingAmount();

            if (!isActiveAndEnabled)
                return;
            
            isBought = false;
            StartProgress();
        }
    }
}
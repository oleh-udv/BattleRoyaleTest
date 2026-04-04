namespace Scripts.Zones
{
    using System;
    using System.Collections;
    using Currencises;
    using Units.Player;
    using UnityEngine;
    using Zenject;

    [RequireComponent(typeof(Collider))]
    public class BuyZone : MonoBehaviour
    {
        [Inject] 
        private Wallet Wallet { get; set; }

        [SerializeField] private Collider collider;

        [Header("Price")] 
        [Range(1, 10000)]
        [SerializeField] private int price;

        [Header("Settings")]
        [Range(1, 100)]
        [SerializeField] private int priceForTick = 1;
        [Range(0.05f, 1f)]
        [SerializeField] private float tickTime = 0.1f;
        [SerializeField] private float deactivateTime = 0.25f;
        [SerializeField] private float startWait = 1f;

        public event Action OnTakeResource;
        public event Action OnBought;
        
        private Coroutine buyCoroutine;
        private int remainingAmount;
        private bool isBought;

        private void Start()
        {
            LoadRemainingAmount();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
                StartProgress();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
                StopProgress();
        }

        private void LoadRemainingAmount()
        {
            remainingAmount = price;
        }

        private void StartProgress()
        {
            buyCoroutine = StartCoroutine(Withdraw());
        }

        private void StopProgress()
        {
            if(buyCoroutine != null)
                StopCoroutine(buyCoroutine);
        }

        private IEnumerator Withdraw()
        {
            yield return new WaitForSeconds(startWait);

            int takeValue = 0;
            var tickWait =  new WaitForSeconds(tickTime);
            
            while (!isBought || Wallet.Balance > 0)
            {
                takeValue = Wallet.IsCanTake(priceForTick) ? priceForTick : Wallet.Balance;
                Wallet.Take(takeValue);
                remainingAmount -= takeValue;
                
                OnTakeResource?.Invoke();

                CheckBuy();
                yield return tickWait;
            }
        }

        private void CheckBuy()
        {
            if (remainingAmount > 0)
                return;

            StopProgress();
            
            isBought = true;
            enabled = false;
            collider.enabled = false;
            
            OnBought?.Invoke();
        }
    }
}
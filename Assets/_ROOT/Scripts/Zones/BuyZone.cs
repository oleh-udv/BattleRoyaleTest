namespace Scripts.Zones
{
    using System;
    using System.Collections;
    using Currencises;
    using Units.Player;
    using UnityEngine;
    using Zenject;

    public class BuyZone : MonoBehaviour
    {
        [Inject] 
        private Wallet Wallet { get; set; }

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

        public void Setup()
        {
            gameObject.SetActive(true);
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
            Wallet.Put(10);
            Debug.Log("Money " + Wallet.Balance);
            buyCoroutine = StartCoroutine(Withdraw());
        }

        private void StopProgress()
        {
            if(buyCoroutine != null)
                StopCoroutine(buyCoroutine);
        }

        private IEnumerator Withdraw()
        {
            Debug.Log("Wait " +  startWait);
            yield return new WaitForSeconds(startWait);
            Debug.Log("End wait");

            int takeValue = 0;
            var tickWait =  new WaitForSeconds(tickTime);
            
            while (!isBought || Wallet.Balance > 0)
            {
                Debug.Log("tick");
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

            Debug.Log("Buy");
            StopProgress();
            
            isBought = true;
            enabled = false;
            
            OnBought?.Invoke();
        }
    }
}
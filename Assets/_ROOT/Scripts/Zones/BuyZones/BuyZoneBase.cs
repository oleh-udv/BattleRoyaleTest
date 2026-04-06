namespace Scripts.Zones.BuyZones
{
    using System.Collections;
    using Currencises;
    using DG.Tweening;
    using TMPro;
    using Units.Player;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public abstract class BuyZoneBase : MonoBehaviour
    {
        [Inject] 
        private Wallet Wallet { get; set; }

        [SerializeField] private Collider collider;

        [Header("Settings")]
        [Range(1, 100)]
        [SerializeField] private int priceForTick = 1;
        [Range(0.05f, 1f)]
        [SerializeField] private float tickTime = 0.1f;
        [SerializeField] private float startWait = 1f;
        
        [Header("Animation")]
        [SerializeField] protected Image fillImage;
        [SerializeField] protected TextMeshProUGUI buyProgress;
        [SerializeField] private float fillTime = 0.2f;

        private Tween fillTween;
        private Coroutine buyCoroutine;
        protected int startPrice;
        protected int remainingAmount;
        protected bool isBought;

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

        protected virtual void LoadRemainingAmount()
        {
            startPrice = 0;
            remainingAmount = 0;

            buyProgress.text = (startPrice - remainingAmount) + "/" + startPrice;
        }
        
        protected virtual void Buy()
        {
            isBought = true;
        }
        
        protected void Deactivate()
        {
            gameObject.SetActive(false);
            fillTween?.Kill();
            StopProgress();
        }

        protected void StartProgress()
        {
            buyCoroutine = StartCoroutine(Withdraw());
        }

        protected void StopProgress()
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

                PlayFillAnimation();
                
                CheckBuy();
                yield return tickWait;
            }
        }

        protected void PlayFillAnimation()
        {
            fillTween?.Kill();
            var fillValue = (startPrice - remainingAmount) / (float)startPrice;
            fillTween = fillImage.DOFillAmount(fillValue, fillTime);
            buyProgress.text =  (startPrice - remainingAmount) + "/" + startPrice;
        }

        private void CheckBuy()
        {
            if (remainingAmount > 0)
                return;

            StopProgress();
            Buy();
        }
    }
}
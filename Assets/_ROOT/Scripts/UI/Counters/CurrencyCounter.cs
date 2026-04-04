namespace Scripts.UI.Counters
{
    using Currencises;
    using TMPro;
    using UnityEngine;
    using Zenject;

    public class CurrencyCounter : MonoBehaviour
    {
        [Inject] 
        private Wallet Wallet { get; set; }
        
        [SerializeField] private TextMeshProUGUI counterText;

        private void Start()
        {
            Wallet.OnUpdateValue += UpdateValue;
        }

        private void OnDestroy()
        {
            Wallet.OnUpdateValue -= UpdateValue;
        }

        private void UpdateValue()
        {
            counterText.text = Wallet.Balance.ToString();
        }
    }
}
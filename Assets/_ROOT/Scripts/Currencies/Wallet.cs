namespace Scripts.Currencises
{
    using System;

    public class Wallet
    {
        public int Balance { get; private set; }

        public bool IsCanTake(int value) => Balance >= value;

        public event Action OnUpdateValue;

        public void Put(int value)
        {
            Balance += value;
            OnUpdateValue?.Invoke();
        }

        public void Take(int value)
        {
            if (value > 0 && Balance >= value)
            {
                Balance -= value;
                OnUpdateValue?.Invoke();
            }
        }
    }
}
namespace Scripts.Currencises
{
    public class Wallet
    {
        public int Balance { get; private set; }

        public bool IsCanTake(int value) => Balance >= value;

        public void Put(int value)
        {
            Balance += value;
        }

        public void Take(int value)
        {
            if (value > 0 && Balance >= value)
            {
                Balance -= value;
            }
        }
    }
}
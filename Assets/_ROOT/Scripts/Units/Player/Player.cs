namespace Scripts.Units.Player
{
    using System;
    using Currencises;
    using Pickable;
    using UnityEngine;
    using Zenject;

    public class Player : MonoBehaviour
    {
        [Inject]
        private Wallet Wallet { get; set; }

        private int currentLevel;
        
        public int Level => currentLevel;
        public event Action OnLevelUp; 

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPickable pickable))
            {
                PickUpItem(pickable);
            }
        }

        private void PickUpItem(IPickable pickable)
        {
            pickable.PickUp(transform);

            switch (pickable.GetPickableType())
            {
                case PickableTypes.Currency:
                    Wallet.Put(pickable.GetValue());
                    break;
            }
        }

        public void LevelUp()
        {
            currentLevel++;
            OnLevelUp?.Invoke();
        }
    }
}
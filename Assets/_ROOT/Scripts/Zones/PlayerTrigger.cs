namespace Scripts.Zones
{
    using System;
    using Units.Player;
    using UnityEngine;

    public class PlayerTrigger : MonoBehaviour
    {
        public event Action OnPlayerEnter;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
                OnPlayerEnter?.Invoke();
        }
    }
}
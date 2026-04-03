namespace Scripts.Zones
{
    using System;
    using UnityEngine;

    public class BuyZoneView : MonoBehaviour
    {
        [SerializeField] private BuyZone buyZone;

        private void Start()
        {
            buyZone.OnBought += PlayBoughtAnimation;
        }

        private void OnDestroy()
        {
            buyZone.OnBought -= PlayBoughtAnimation;
        }

        private void PlayBoughtAnimation()
        {
            gameObject.SetActive(false);
        }
    }
}
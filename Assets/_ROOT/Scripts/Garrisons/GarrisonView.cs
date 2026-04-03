namespace Scripts.Garrisons
{
    using System;
    using UnityEngine;

    public class GarrisonView : MonoBehaviour
    {
        [SerializeField] private Garrison garrison;

        private void Awake()
        {
            garrison.OnActivate += ShowAppearance;
            garrison.OnDeactivate += Hide;
        }

        private void OnDestroy()
        {
            garrison.OnActivate -= ShowAppearance;
            garrison.OnDeactivate -= Hide;
        }

        private void ShowAppearance()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
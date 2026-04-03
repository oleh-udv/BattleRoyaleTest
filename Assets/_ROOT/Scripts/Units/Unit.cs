namespace Scripts.Units
{
    using System;
    using Settings;
    using UnityEngine;

    public abstract class Unit : MonoBehaviour
    {
        public event Action OnSetup;
        
        public void Setup(UnitLevelingSettings.LevelingSettings settings)
        {
            gameObject.SetActive(true);
            OnSetup?.Invoke();
        }
    }
}
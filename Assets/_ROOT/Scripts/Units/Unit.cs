namespace Scripts.Units
{
    using System;
    using Movement;
    using Settings;
    using UnityEngine;

    public abstract class Unit : MonoBehaviour
    {
        [SerializeField] protected UnitMovement unitMovement;
        public event Action OnSetup;
        
        public void Setup(LevelingSettings settings)
        {
            gameObject.SetActive(true);
            OnSetup?.Invoke();
        }

        public void MoveToPoint(Vector3 point)
        {
            unitMovement.SetMovePoint(point);
        }
    }
}
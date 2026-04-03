namespace Scripts.Zones
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ArmyFormationZone : MonoBehaviour
    {
        [SerializeField] private List<Transform> unitPoints;
        [SerializeField] private int startPointsCount;

        private List<Transform> activePoints;
        private int freePointIndex;

        public event Action OnFull;

        private void Start()
        {
            ActivatePoints(startPointsCount);
        }

        public Vector3 GetFreePoint()
        {
            if(freePointIndex >= activePoints.Count)
                return Vector3.zero;
            
            var point = activePoints[freePointIndex];
            freePointIndex++;
            if (freePointIndex >= activePoints.Count)
                OnFull?.Invoke();

            return point.position;
        }

        public void ClearPoints()
        {
            freePointIndex = 0;
        }

        private void Upgrade()
        {
            
        }

        private void ActivatePoints(int count)
        {
            activePoints =  new List<Transform>(count);

            for (int i = 0; i < count; i++)
            {
                activePoints.Add(unitPoints[i]);
                unitPoints[i].gameObject.SetActive(true);
            }
        }
    }
}
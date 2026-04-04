namespace Scripts.Units.EnemyArmy
{
    using PlayerArmy;
    using UnityEngine;

    public class EnemyArmyUnit : Unit
    {
        private Vector3 startPoint;

        protected override void Start()
        {
            base.Start();
            
            startPoint = transform.position;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerArmyUnit armyUnit))
                DetectEnemy(armyUnit);
        }

        public void ReturnToStartPoint()
        {
            MoveToPoint(startPoint);
        }
    }
}
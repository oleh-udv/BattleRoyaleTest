namespace Scripts.Units.EnemyArmy
{
    using PlayerArmy;
    using UnityEngine;

    public class EnemyArmyUnit : Unit
    {
        private Vector3 startPoint;
        private Vector3 startRotation;

        protected override void Start()
        {
            base.Start();
            
            startPoint = transform.position;
            startRotation = transform.rotation.eulerAngles;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerArmyUnit armyUnit))
                DetectEnemy(armyUnit);
        }
    }
}
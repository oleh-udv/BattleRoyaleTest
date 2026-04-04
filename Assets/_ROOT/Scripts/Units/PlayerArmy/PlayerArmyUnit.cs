namespace Scripts.Units.PlayerArmy
{
    using EnemyArmy;
    using UnityEngine;

    public class PlayerArmyUnit : Unit
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyArmyUnit armyUnit))
                DetectEnemy(armyUnit);
        }
    }
}
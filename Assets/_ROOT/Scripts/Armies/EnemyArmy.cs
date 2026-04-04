namespace Scripts.Armies
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class EnemyArmy : MonoBehaviour
    {
        [SerializeField] private List<EnemyGroup> enemyGroups;
        
        public Vector3 GetAttackPoint()
        {
            var firstAliveGroup = enemyGroups.FirstOrDefault(g => g.IsAlive);
            if (firstAliveGroup == null)
            {
                LoseArmy();
                return Vector3.zero;
            }

            firstAliveGroup.SetWaitUnits(true);
            return firstAliveGroup.transform.position;
        }

        private void LoseArmy()
        {
            //
        }
    }
}
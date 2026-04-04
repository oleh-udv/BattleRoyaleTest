namespace Scripts.Armies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Units;
    using UnityEngine;

    public class EnemyArmy : MonoBehaviour
    {
        [SerializeField] private List<EnemyGroup> enemyGroups;

        private List<Unit> attackedUnits = new();
        
        private EnemyGroup attackedGroup;

        private void OnDestroy()
        {
            attackedUnits.ForEach(u => u.OnDied -= CheckEndBattle);
            attackedUnits.Clear();
        }

        public Vector3 GetAttackPoint()
        {
            attackedGroup = enemyGroups.FirstOrDefault(g => g.IsAlive);
            if (attackedGroup == null)
            {
                LoseArmy();
                return Vector3.zero;
            }

            attackedGroup.SetWaitUnits(true);
            return attackedGroup.transform.position;
        }

        public void SetAttackedUnits(List<Unit> units)
        {
            attackedUnits.AddRange(units);
            attackedUnits.ForEach(u => u.OnDied += CheckEndBattle);
        }

        private void CheckEndBattle()
        {
            if (!attackedUnits.Any(u => u.IsAlive))
                EndBattle();
        }
        
        private void EndBattle()
        {
            Debug.Log("EnemyEnd");
            attackedUnits.ForEach(u => u.OnDied -= CheckEndBattle);
            attackedUnits.Clear();

            if (attackedGroup && attackedGroup.IsAlive)
            {
                Debug.Log("Command");
                attackedGroup.SetWaitUnits(false);
                attackedGroup.ReturnUnits();
            }
        }

        private void LoseArmy()
        {
            //
        }
    }
}
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
        public event Action<bool> OnGroupLose;

        private void Start()
        {
            enemyGroups.ForEach(g => g.OnGroupLose += GroupLose);
        }

        private void OnDestroy()
        {
            enemyGroups.ForEach(g => g.OnGroupLose -= GroupLose);
            attackedUnits.ForEach(u => u.OnDied -= CheckEndBattle);
            attackedUnits.Clear();
        }

        public Vector3 GetAttackPoint()
        {
            attackedGroup = enemyGroups.FirstOrDefault(g => g.IsAlive);
            if (attackedGroup == null)
                return Vector3.zero;

            attackedGroup.SetWaitUnits(true);
            return attackedGroup.transform.position;
        }

        public void SetAttackedUnits(List<Unit> units)
        {
            attackedUnits.Clear();
            attackedUnits.AddRange(units);
            attackedUnits.ForEach(u => u.OnDied += CheckEndBattle);
        }

        private void GroupLose()
        {
            var armyDied = enemyGroups.Any(g => g.IsAlive);
            OnGroupLose?.Invoke(armyDied);
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
    }
}
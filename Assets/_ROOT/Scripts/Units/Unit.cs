namespace Scripts.Units
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Bullets;
    using Movement;
    using Settings;
    using UnityEngine;
    using Zenject;

    public abstract class Unit : MonoBehaviour, IDamageable
    {
        [Inject] 
        private BulletsFactory BulletsFactory { get; set; }

        [SerializeField] private UnitMovement unitMovement;
        [SerializeField] private Collider attackVisionCollider;
        [SerializeField] private CharacterController characterController;
        [Header("Bullets")]
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform bulletContainer;
        [SerializeField] private int startBulletsCount;

        private int hp;
        private int damage;
        private float shootInterval;
        private Coroutine shootCoroutine;
        private BulletsPool bulletsPool;
        
        protected List<Unit> attackOrder = new();

        public int Level { get; private set; }

        public bool IsAlive { get; private set; } = true;

        public event Action OnSetup;
        public event Action OnShoot;
        public event Action OnDied;

        protected virtual void Start()
        {
            bulletsPool = new BulletsPool(bulletPrefab, bulletContainer, startBulletsCount, BulletsFactory);
        }

        public void Setup(LevelingSettings settings)
        {
            IsAlive = true;
            attackOrder.Clear();
            characterController.enabled = true;

            Level = settings.Level;
            hp = settings.UnitSettings.HP;
            damage = settings.UnitSettings.Damage;
            shootInterval = settings.UnitSettings.ShootInterval;
            
            gameObject.SetActive(true);
            OnSetup?.Invoke();
        }

        public void MoveToPoint(Vector3 point)
        {
            unitMovement.SetMovePoint(point);
        }

        public void SetReadyToFight(bool isReady)
        {
            attackVisionCollider.enabled = isReady;
        }

        protected void DetectEnemy(Unit unit)
        {
            if (attackOrder.Contains(unit))
                return;

            attackOrder.Add(unit);
            Attack();
        }

        private void Attack()
        {
            unitMovement.StopMoveToPoint();
            var unit = attackOrder.FirstOrDefault(u => u.IsAlive);
            
            if (unit == null)
                StopAttack();
            else
                shootCoroutine = StartCoroutine(ShootRoutine(unit));
        }

        private void StopAttack()
        {
            if(shootCoroutine != null)
                StopCoroutine(shootCoroutine);
        }

        private IEnumerator ShootRoutine(Unit unit)
        {
            var interval = new WaitForSeconds(shootInterval);
            while (unit.IsAlive)
            {
                yield return interval;
                Shoot();
            }
        }

        public void Shoot()
        {
            OnShoot?.Invoke();
        }

        public void GetDamage(int damage)
        {
            hp -= damage;
            if (hp <= 0)
                Die();
        }

        public void Die()
        {
            characterController.enabled = false;
            IsAlive = false;
            
            OnDied?.Invoke();
        }
    }
}
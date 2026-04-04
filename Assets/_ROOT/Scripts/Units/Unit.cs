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
        
        [Header("Shoot")]
        [SerializeField] private Transform shootPoint;

        private int hp;
        private int damage;
        private float shootInterval;
        private Coroutine shootCoroutine;
        private BulletsPool bulletsPool;
        private bool isInAttack;
        
        protected List<Unit> attackOrder = new();

        public UnitMovement UnitMovement => unitMovement;
        public int Level { get; private set; }
        public bool IsAlive { get; private set; } = true;

        public event Action OnSetup;
        public event Action OnShoot;
        public event Action OnDied;
        public event Action OnEndBattle;

        protected virtual void Start()
        {
            bulletsPool = new BulletsPool(bulletPrefab, bulletContainer, startBulletsCount, BulletsFactory);
        }

        public void Setup(LevelingSettings settings)
        {
            IsAlive = true;
            attackOrder.Clear();
            characterController.enabled = true;
            unitMovement.enabled = true;
            
            Level = settings.Level;
            hp = settings.UnitSettings.HP;
            damage = settings.UnitSettings.Damage;
            shootInterval = settings.UnitSettings.ShootInterval;
            
            StopAttack();
            gameObject.SetActive(true);
            
            OnSetup?.Invoke();
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
            if(!isInAttack)
                Attack();
        }

        private void Attack()
        {
            isInAttack = true;
            unitMovement.StopMoveToPoint();
            shootCoroutine = StartCoroutine(ShootRoutine());
        }

        private void StopAttack()
        {
            if(shootCoroutine != null)
                StopCoroutine(shootCoroutine);
            
            attackOrder.Clear();
            isInAttack = false;
            OnEndBattle?.Invoke();
        }

        private IEnumerator ShootRoutine()
        {
            var interval = new WaitForSeconds(shootInterval);
            OnShoot?.Invoke();

            while (attackOrder.Any(u => u.IsAlive))
            {
                var unit = attackOrder.FirstOrDefault(u => u.IsAlive);
                
                while (unit.IsAlive)
                {
                    yield return interval;
                    Shoot(unit);
                }
            }

            StopAttack();
            unitMovement.MoveToLastPoint();
        }

        private void Shoot(Unit unit)
        {
            if(!unit.IsAlive)
                return;
            
            var bullet = bulletsPool.GetFreeElement();
            bullet.transform.position = shootPoint.position;
            bullet.transform.SetParent(transform.parent);
            bullet.Setup(unit, bulletContainer, unit.transform.position, damage);
            bullet.gameObject.SetActive(true);
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
            unitMovement.enabled = false;
            
            if(shootCoroutine != null)
                StopCoroutine(shootCoroutine);
            
            OnDied?.Invoke();
        }
    }
}
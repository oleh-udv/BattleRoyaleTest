namespace Scripts.Bullets
{
    using System;
    using Units;
    using UnityEngine;
    using DG.Tweening;
    
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed;
        
        private IDamageable damageable;
        private Vector3 movePoint;
        private int damage;
        
        private Sequence sequence;
        
        public void Setup(IDamageable damageable, Vector3 endPoint, int damage)
        {
            this.damageable = damageable;
            this.damage = damage;
            
            movePoint = endPoint;
            movePoint.y = transform.position.y;

            gameObject.SetActive(true);
            
            MoveToPoint();
        }

        private void OnDestroy()
        {
            sequence.Kill();
        }

        private void MoveToPoint()
        {
            var distanse = Vector3.Distance(transform.position, movePoint);
            sequence = DOTween.Sequence();
            
            sequence.Append(transform.DOMove(movePoint, distanse / speed));
            sequence.OnComplete(DealDamage);
        }

        private void DealDamage()
        {
            if(damageable != null)
                damageable.GetDamage(damage);
            
            gameObject.SetActive(false);
        }
    }
}
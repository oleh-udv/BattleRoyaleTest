namespace Scripts.Bullets
{
    using Units;
    using UnityEngine;
    using DG.Tweening;
    
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed;
        
        private IDamageable damageable;
        private Transform parent;
        private Vector3 movePoint;
        private int damage;
        
        private bool isActive;
        private Sequence sequence;
        
        public bool IsActive => isActive;
        
        public void Setup(IDamageable damageable, Transform parent, Vector3 endPoint, int damage)
        {
            isActive = true;
            this.damageable = damageable;
            this.damage = damage;
            this.parent = parent;
            
            movePoint = endPoint;
            movePoint.y = transform.position.y;

            gameObject.SetActive(true);
            
            MoveToPoint();
        }

        private void MoveToPoint()
        {
            sequence?.Kill();
            
            var distance = Vector3.Distance(transform.position, movePoint);
            
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(movePoint, distance / speed));
            sequence.OnComplete(EndMove);
        }

        private void EndMove()
        {
            DealDamage();
            Deactivate();
        }

        private void DealDamage()
        {
            if(damageable != null)
                damageable.GetDamage(damage);
        }

        private void Deactivate()
        {
            sequence?.Kill();
            gameObject.SetActive(false);
            transform.SetParent(parent, true);

            isActive = false;
        }
    }
}
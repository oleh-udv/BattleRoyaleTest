namespace Scripts.Bullets
{
    using Infrastructure.PoolBase;
    using UnityEngine;
    using Zenject;

    public class BulletsPool : PoolObjectsBase<Bullet>
    {
        private IFactory<Bullet, Transform, Bullet> factory;
        
        public BulletsPool(Bullet prefab, Transform container, int count, 
            IFactory<Bullet, Transform, Bullet> factory) : base(prefab, container)
        {
            this.factory = factory;
            CreatePool(count);
        }
        
        protected override Bullet CreateObject()
        {
            var createdObject = factory.Create(prefab, container);
            pool.Add(createdObject);
            createdObject.gameObject.SetActive(false);
            return createdObject;
        }
    }
}
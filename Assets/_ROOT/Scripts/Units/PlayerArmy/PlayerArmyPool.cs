namespace Scripts.Units.PlayerArmy
{
    using Infrastructure.PoolBase;
    using UnityEngine;
    using Zenject;

    public class PlayerArmyPool : PoolObjectsBase<Unit>
    {
        private IFactory<Unit, Transform, Unit> factory;
        
        public PlayerArmyPool(Unit prefab, Transform container, int count, IFactory<Unit, Transform, Unit> factory)
            : base(prefab, container)
        {
            this.factory = factory;
            CreatePool(count);
        }
        

        protected override Unit CreateObject()
        {
            var createdObject = factory.Create(prefab, container);
            pool.Add(createdObject);
            createdObject.gameObject.SetActive(false);
            return createdObject;
        }
    }
}
namespace Scripts.Units.PlayerArmy
{
    using Infrastructure.PoolBase;
    using UnityEngine;
    using Zenject;

    public class PlayerArmyPool : PoolObjectsBase<Unit>
    {
        private IFactory<Unit, Unit> factory;
        
        public PlayerArmyPool(Unit prefab, Transform container, int count, IFactory<Unit, Unit> factory)
            : base(prefab, container, count)
        {
            this.factory = factory;
        }

        protected override Unit CreateObject()
        {
            return factory.Create(prefab);
        }
    }
}
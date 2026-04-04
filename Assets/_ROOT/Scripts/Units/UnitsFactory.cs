namespace Scripts.Units
{
    using Units;
    using UnityEngine;
    using Zenject;

    public class UnitsFactory : IFactory<Unit, Transform, Unit>
    {
        [Inject] 
        private IInstantiator Instantiator { get; set; }
        
        public Unit Create(Unit unit, Transform transform)
        {
            return Instantiator.InstantiatePrefabForComponent<Unit>(unit, transform);
        }
    }
}
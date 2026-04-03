namespace Scripts.PlayerArmy
{
    using Units;
    using UnityEngine;
    using Zenject;

    public class PlayerUnitsFactory : IFactory<Unit, Transform, Unit>
    {
        [Inject] 
        private IInstantiator Instantiator { get; set; }
        
        public Unit Create(Unit unit, Transform transform)
        {
            return Instantiator.InstantiatePrefabForComponent<Unit>(unit, transform);
        }
    }
}
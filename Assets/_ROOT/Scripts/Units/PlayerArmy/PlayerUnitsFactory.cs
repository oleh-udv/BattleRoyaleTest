namespace Scripts.Units.PlayerArmy
{
    using Zenject;

    public class PlayerUnitsFactory : IFactory<Unit, Unit>
    {
        [Inject] 
        private IInstantiator Instantiator { get; set; }
        
        public Unit Create(Unit unit)
        {
            return Instantiator.InstantiatePrefabForComponent<Unit>(unit);
        }
    }
}
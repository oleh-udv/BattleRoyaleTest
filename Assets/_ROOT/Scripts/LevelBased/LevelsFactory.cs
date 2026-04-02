namespace Scripts.LevelBased
{
    using Zenject;

    public class LevelsFactory : IFactory<Level, Level>
    {
        [Inject] 
        private IInstantiator Instantiator { get; set; }

        public Level Create(Level prefab)
        {
            return Instantiator.InstantiatePrefabForComponent<Level>(prefab);
        }
    }
}
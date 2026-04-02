namespace Scripts.Infrastructure
{
    using LevelBased;
    using UnityEngine;
    using Zenject;

    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private BaseLevelCreator levelCreator;
        
        public override void InstallBindings()
        {
            BindGameServices();
            BindFactories();
        }

        private void BindGameServices()
        {
            Container.Bind<BaseLevelCreator>().FromInstance(levelCreator);
        }

        private void BindFactories()
        {
            Container.BindInterfacesAndSelfTo<LevelsFactory>().AsSingle();
        }
    }
}
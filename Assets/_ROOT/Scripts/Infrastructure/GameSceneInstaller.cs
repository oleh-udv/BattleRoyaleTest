namespace Scripts.Infrastructure
{
    using Camera;
    using Input;
    using LevelBased;
    using UnityEngine;
    using Zenject;

    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private BaseLevelCreator levelCreator;
        [SerializeField] private VirtualCamera virtualCamera;
        
        public override void InstallBindings()
        {
            BindGameServices();
            BindFactories();
            BindCamera();
        }

        private void BindGameServices()
        {
            Container.Bind<BaseLevelCreator>().FromInstance(levelCreator).AsSingle();
            
            Container.Bind<IInputProvider>().To<InputProvider>().AsSingle();
        }

        private void BindFactories()
        {
            Container.BindInterfacesAndSelfTo<LevelsFactory>().AsSingle();
        }

        private void BindCamera()
        {
            Container.Bind<VirtualCamera>().FromInstance(virtualCamera).AsSingle();
        }
    }
}
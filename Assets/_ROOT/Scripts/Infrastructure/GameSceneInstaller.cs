namespace Scripts.Infrastructure
{
    using Bullets;
    using Camera;
    using Currencises;
    using Input;
    using LevelBased;
    using Units;
    using UnityEngine;
    using Zenject;

    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private LevelCreatorBase levelCreator;
        [SerializeField] private VirtualCamera virtualCamera;
        
        public override void InstallBindings()
        {
            BindGameServices();
            BindFactories();
            BindCamera();

            BindCurrencies();
        }

        private void BindGameServices()
        {
            Container.Bind<LevelCreatorBase>().FromInstance(levelCreator).AsSingle();
            
            Container.Bind<IInputProvider>().To<InputProvider>().AsSingle();
        }

        private void BindFactories()
        {
            Container.BindInterfacesAndSelfTo<LevelsFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnitsFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<BulletsFactory>().AsSingle();
        }

        private void BindCamera()
        {
            Container.Bind<VirtualCamera>().FromInstance(virtualCamera).AsSingle();
        }

        private void BindCurrencies()
        {
            Container.Bind<Wallet>().AsSingle();
        }
    }
}
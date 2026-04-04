namespace Scripts.Infrastructure
{
    using Bullets;
    using Camera;
    using Currencises;
    using Input;
    using LevelBased;
    using Pickable;
    using UI.Counters;
    using Units;
    using Units.Animations;
    using UnityEngine;
    using Zenject;

    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private LevelCreatorBase levelCreator;
        [SerializeField] private VirtualCamera virtualCamera;
        [SerializeField] private CurrencyCounter currencyCounter;
        
        public override void InstallBindings()
        {
            BindGameServices();
            BindFactories();
            BindCamera();

            BindCurrencies();

            BindConstants();
            BindUI();
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
            Container.BindInterfacesAndSelfTo<PickableCurrencyFactory>().AsSingle();
        }

        private void BindCamera()
        {
            Container.Bind<VirtualCamera>().FromInstance(virtualCamera).AsSingle();
        }

        private void BindCurrencies()
        {
            Container.Bind<Wallet>().AsSingle();
        }

        private void BindConstants()
        {
            Container.Bind<UnitAnimationConstants>().AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<CurrencyCounter>().FromInstance(currencyCounter).AsSingle();
        }
    }
}
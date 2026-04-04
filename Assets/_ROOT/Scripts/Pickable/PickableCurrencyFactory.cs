namespace Scripts.Pickable
{
    using UnityEngine;
    using Zenject;

    public class PickableCurrencyFactory: IFactory<PickableCurrency, Transform, PickableCurrency>
    {
        [Inject] 
        private IInstantiator Instantiator { get; set; }

        public PickableCurrency Create(PickableCurrency prefab, Transform parent)
        {
            return Instantiator.InstantiatePrefabForComponent<PickableCurrency>(prefab, parent);
        }
    }
}
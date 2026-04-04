namespace Scripts.Units
{
    using Pickable;
    using UnityEngine;
    using Zenject;

    public class PickableForKillUnit : MonoBehaviour
    {
        [Inject] 
        private PickableCurrencyFactory PickableCurrencyFactory { get; set; }
        
        [SerializeField] private Unit unit;
        [SerializeField] private PickableCurrency prefab;

        private void Start()
        {
            unit.OnDied += SpawnItem;
        }

        private void OnDestroy()
        {
            unit.OnDied -= SpawnItem;
        }

        private void SpawnItem()
        {
            var item = PickableCurrencyFactory.Create(prefab, transform.parent);
            item.transform.position = transform.position;
            item.PlayFallAnimation();
        }
    }
}
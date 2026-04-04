namespace Scripts.Bullets
{
    using UnityEngine;
    using Zenject;

    public class BulletsFactory : IFactory<Bullet, Transform, Bullet>
    {
        [Inject] 
        private IInstantiator Instantiator { get; set; }
        
        public Bullet Create(Bullet prefab, Transform parent)
        {
            return Instantiator.InstantiatePrefabForComponent<Bullet>(prefab, parent);
        }
    }
}
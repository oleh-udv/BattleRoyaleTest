namespace Scripts.Units.Settings
{
    using Bullets;
    using UnityEngine;

    [CreateAssetMenu(fileName = nameof(UnitSettings), menuName = "Game/Settings/UnitSettings")]
    public class UnitSettings : ScriptableObject
    {
        [SerializeField] private int hp;
        [SerializeField] private int damage;
        [SerializeField] private float shootInterval;

        public int HP => hp;
        public int Damage => damage;
        public float ShootInterval => shootInterval;
    }
}
namespace Scripts.Units.Settings
{
    using UnityEngine;

    [System.Serializable]
    public class LevelingSettings
    {
        [SerializeField] private int level;
        [SerializeField] private UnitSettings unitSettings;
        
        public int Level => level;
        public UnitSettings UnitSettings => unitSettings;
    }
}
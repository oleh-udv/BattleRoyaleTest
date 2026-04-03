namespace Scripts.Units.Settings
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    [CreateAssetMenu(fileName = nameof(UnitLevelingSettings), menuName = "Game/Settings/UnitLevelingSettings")]
    public class UnitLevelingSettings : ScriptableObject
    {
        [SerializeField] private List<LevelingSettings> levelingSettings;

        public LevelingSettings GetSettingsByLevel(int level)
        {
            return levelingSettings.FirstOrDefault(s => s.Level == level);
        }

        [System.Serializable]
        public struct LevelingSettings
        {
            public int Level;
            public UnitSettings UnitSettings;
        }
    }
}
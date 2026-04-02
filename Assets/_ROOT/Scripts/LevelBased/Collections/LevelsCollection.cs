namespace Scripts.LevelBased.Collections
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = nameof(LevelsCollection), menuName = "Game/Collections/LevelsCollection", order = 0)]
    public class LevelsCollection : ScriptableObject
    {
        [SerializeField] private List<Level> levels;
        
        public Level GetLevelByIndex(int index)
        {
            return levels[index];
        }
    }
}
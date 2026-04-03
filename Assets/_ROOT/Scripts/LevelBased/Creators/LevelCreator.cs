namespace Scripts.LevelBased
{
    using Collections;
    using UnityEngine;
    using Zenject;

    public class LevelCreator : LevelCreatorBase
    {
        [Inject] 
        private LevelsFactory LevelsFactory { get; set; }

        [SerializeField] private LevelsCollection levelsCollection;

        private Level currentLevel;
            
        private void Awake()
        {
            CreateLevel();
        }
        
        public override void CreateLevel()
        {
            currentLevel = LevelsFactory.Create(levelsCollection.GetLevelByIndex(0));
        }
    }
}
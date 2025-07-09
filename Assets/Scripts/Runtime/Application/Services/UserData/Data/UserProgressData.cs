using System;

namespace Application.Services.UserData
{
    [Serializable]
    public class UserProgressData
    {
        private int _currentLevel = 1;
        
        public int HighestLevelsUnlocked = 1;
        
        public int CurrentLevel
        {
            get => _currentLevel;
            set
            {
                _currentLevel = value;
                TryToIncreaseHighestLevel();
            }
        }

        public void IncreaseCurrentLevel() =>
                CurrentLevel++;

        public void SetCurrentLevel(int level) =>
                CurrentLevel = level;

        private void TryToIncreaseHighestLevel()
        {
            if(_currentLevel > HighestLevelsUnlocked)
                HighestLevelsUnlocked = _currentLevel;
        }
    }
}
using Application.UI;
using UnityEngine;

namespace Runtime.Application.UI.LevelSelectionButtonHandlers
{
    public class LevelSelectionButtonAdjuster : ILevelSelectionButtonAdjuster
    {
        private readonly LevelSelectionModel _levelSelectionModel;
        
        public LevelSelectionButtonAdjuster(LevelSelectionModel levelSelectionModel) =>
                _levelSelectionModel = levelSelectionModel;

        public void AdjustButton(LevelSelectionButton newButton, int level, RectTransform container)
        {
            newButton.transform.SetParent(container);
            newButton.transform.localScale = Vector3.one;
            newButton.SetLevel(level);
        }

        public void SetButtonState(LevelSelectionButton levelSelectionButton)
        {
            if(levelSelectionButton.GetLevel() == _levelSelectionModel.SelectedLevel)
            {
                _levelSelectionModel.UpdatePreviousLevelSelectionButton(levelSelectionButton);
                levelSelectionButton.UpdateSelection(true);
            }

            if(levelSelectionButton.GetLevel() > _levelSelectionModel.SelectedLevel)
                levelSelectionButton.SetLocked(_levelSelectionModel.LockedSprite);
        }

        public void UpdateSelectedLevel(LevelSelectionButton levelSelectionButton, int level)
        {
            if(SetLevel(level))
                return;

            if(_levelSelectionModel.PreviousLevelSelectionButton)
                _levelSelectionModel.PreviousLevelSelectionButton.UpdateSelection(false);

            levelSelectionButton.UpdateSelection(true);
            _levelSelectionModel.PreviousLevelSelectionButton = levelSelectionButton;
        }
        
        private bool SetLevel(int level)
        {
            if(_levelSelectionModel.SelectedLevel == level)
                return true;

            _levelSelectionModel.UpdateSelectedLevel(level);

            return false;
        }
    }
}
using System.Collections.Generic;
using Runtime.Application.UI.LevelSelectionButtonHandlers;
using UnityEngine;

namespace Application.UI
{
    public class LevelSelectionModel 
    {
        public readonly List<LevelSelectionButton> LevelSelectionButtons = new();
        public readonly Sprite LockedSprite;

        public int SelectedLevel = 1;
        public LevelSelectionButton PreviousLevelSelectionButton;
        public RectTransform Container;

        public LevelSelectionModel(Sprite lockedSprite) =>
                LockedSprite = lockedSprite;

        public void SetButtonsContainer(RectTransform container) =>
                Container = container;
        
        public void UpdateSelectedLevel(int level)
            => SelectedLevel = level;
        
        public void UpdatePreviousLevelSelectionButton(LevelSelectionButton previousLevelSelectionButton)
            => PreviousLevelSelectionButton = previousLevelSelectionButton;
        
        public void AddLevelSelectionButton(LevelSelectionButton levelSelectionButton)
            => LevelSelectionButtons.Add(levelSelectionButton);

        public void ClearLevelSelectionButtons()
            => LevelSelectionButtons.Clear();
    }
}
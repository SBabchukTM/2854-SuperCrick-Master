using UnityEngine;

namespace Runtime.Application.UI.LevelSelectionButtonHandlers
{
    public interface ILevelSelectionButtonAdjuster
    {
        void AdjustButton(LevelSelectionButton newButton, int level, RectTransform container);

        void SetButtonState(LevelSelectionButton levelSelectionButton);

        void UpdateSelectedLevel(LevelSelectionButton levelSelectionButton, int level);
    }
}
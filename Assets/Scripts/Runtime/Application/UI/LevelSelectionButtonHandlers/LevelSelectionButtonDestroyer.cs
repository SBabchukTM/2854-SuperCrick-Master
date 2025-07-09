using Application.UI;
using UnityEngine;

namespace Runtime.Application.UI.LevelSelectionButtonHandlers
{
    public class LevelSelectionButtonDestroyer : ILevelSelectionButtonDestroyer
    {
        private readonly LevelSelectionModel _levelSelectionModel;
        
        public LevelSelectionButtonDestroyer(LevelSelectionModel levelSelectionModel) =>
                _levelSelectionModel = levelSelectionModel;
        
        public void DestroyAllButtons()
        {
            foreach (var levelSelectionButton in _levelSelectionModel.LevelSelectionButtons)
            {
                if(levelSelectionButton)
                {
                    levelSelectionButton.Button.onClick.RemoveAllListeners();
                    Object.Destroy(levelSelectionButton.gameObject);
                }
            }
            
            _levelSelectionModel.ClearLevelSelectionButtons();
        }
    }
}
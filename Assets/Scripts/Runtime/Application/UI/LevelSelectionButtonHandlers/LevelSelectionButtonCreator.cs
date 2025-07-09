using Application.UI;
using Core.Factory;
using Cysharp.Threading.Tasks;
using R3;

namespace Runtime.Application.UI.LevelSelectionButtonHandlers
{
    public class LevelSelectionButtonCreator : ILevelSelectionButtonCreator
    {
        private readonly GameObjectFactory _gameObjectFactory;
        private readonly LevelSelectionModel _levelSelectionModel;
        private readonly ILevelSelectionButtonAdjuster _levelSelectionButtonAdjuster;

        public LevelSelectionButtonCreator(GameObjectFactory gameObjectFactory, LevelSelectionModel levelSelectionModel,
                ILevelSelectionButtonAdjuster levelSelectionButtonAdjuster)
        {
            _levelSelectionButtonAdjuster = levelSelectionButtonAdjuster;
            _gameObjectFactory = gameObjectFactory;
            _levelSelectionModel = levelSelectionModel;
        }
        
        public async UniTask CreateSkinSelectionButtons(int levelCount)
        {
            for (var levelIndex = 0; levelIndex < levelCount; levelIndex++)
            {
                var newButton = await CreateButton();
                AddToList(newButton);
                AdjustButton(newButton, levelIndex);
                SubscribeButton(newButton);
            }
        }

        private void SubscribeButton(LevelSelectionButton levelSelectionButton) =>
                levelSelectionButton.LevelSelectedSubject.Subscribe(tuple =>
                {
                    var (levelButton, index) = tuple;

                    _levelSelectionButtonAdjuster.UpdateSelectedLevel(levelButton, index);
                });

        private void AdjustButton(LevelSelectionButton newButton, int levelIndex)
        {
            _levelSelectionButtonAdjuster.AdjustButton(newButton, levelIndex + 1, _levelSelectionModel.Container);
            _levelSelectionButtonAdjuster.SetButtonState(newButton);
        }

        private async UniTask<LevelSelectionButton> CreateButton() =>
                await _gameObjectFactory.Create<LevelSelectionButton>(ConstGame.LevelSelectionButton);
        
        private void AddToList(LevelSelectionButton newButton) 
            => _levelSelectionModel.AddLevelSelectionButton(newButton);
    }
}
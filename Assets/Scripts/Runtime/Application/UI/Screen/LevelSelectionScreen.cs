using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Runtime.Application.UI.LevelSelectionButtonHandlers;
using Runtime.Core.Extensions;
using UnityEngine;
using Zenject;

namespace Application.UI
{
    public class LevelSelectionScreen : UiScreen
    {
        public readonly Subject<Unit> SettingsPressedSubject = new();
        public readonly Subject<Unit> LeavePressedSubject = new();
        public readonly Subject<int> StartLevelPressedSubject = new();
        
        private ILevelSelectionButtonCreator _levelSelectionButtonCreator;
        private ILevelSelectionButtonDestroyer _levelSelectionButtonDestroyer;
        private LevelSelectionModel _levelSelectionModel;

        [SerializeField] private SimpleButton _settingsButton;
        [SerializeField] private SimpleButton _goBackButton;
        [SerializeField] private SimpleButton _playButton;
        [SerializeField] private RectTransform _buttonContainer;

        [Inject]
        public void Construct(ILevelSelectionButtonCreator levelSelectionButtonCreator, ILevelSelectionButtonDestroyer levelSelectionButtonDestroyer,
                LevelSelectionModel levelSelectionModel)
        {
            _levelSelectionButtonCreator = levelSelectionButtonCreator;
            _levelSelectionButtonDestroyer = levelSelectionButtonDestroyer;
            _levelSelectionModel = levelSelectionModel;
        }

        private void OnDestroy() =>
                _levelSelectionButtonDestroyer.DestroyAllButtons();

        public override UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            _goBackButton.Button.SubscribeButtonClick(LeavePressedSubject);
            _settingsButton.Button.SubscribeButtonClick(SettingsPressedSubject);
            _playButton.Button.SubscribeButtonClick(StartLevelPressedSubject, _levelSelectionModel.SelectedLevel);           

            _levelSelectionModel.SetButtonsContainer(_buttonContainer);
            
            return base.ShowAsync(cancellationToken);
        }

        public void Initialize(int selectedLevel) =>
                _levelSelectionModel.UpdateSelectedLevel(selectedLevel);

        public void CreateSkinSelectionButtons(int levels)
        {
            _levelSelectionButtonDestroyer.DestroyAllButtons();
            _levelSelectionButtonCreator.CreateSkinSelectionButtons(levels).Forget();
        }
    }
}
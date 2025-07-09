using System.Threading;
using Application.Game;
using Application.Services.UserData;
using Application.UI;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using R3;
using Runtime.Application.ApplicationStates.Game.Controllers;
using Runtime.Core.Extensions;
using ILogger = Core.ILogger;

namespace Application.GameStateMachine
{
    public class LevelSelectionController : StateController
    {
        private readonly StateMachine _stateMachine;
        private readonly IUiService _uiService;
        private readonly UserDataService _userDataService;
        private readonly StartSettingsController _startSettingsController;

        private LevelSelectionScreen _levelSelectionScreen;
        private CancellationTokenSource _cancellationTokenSource;

        public LevelSelectionController(ILogger logger, IUiService uiService, UserDataService userDataService,
                StartSettingsController startSettingsController) : base(logger)
        {
            _startSettingsController = startSettingsController;
            _uiService = uiService;
            _userDataService = userDataService;
        }

        public override UniTask Enter(CancellationToken cancellationToken = default)
        {
            _cancellationTokenSource = new();
            
            ShowScreen(cancellationToken);
            Subscribe();
            
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            _cancellationTokenSource.Cancel();
            
            await _uiService.HideScreen(ConstScreens.LevelSelectionScreen);
        }

        private void ShowScreen(CancellationToken cancellationToken)
        {
            _levelSelectionScreen = _uiService.GetScreen<LevelSelectionScreen>(ConstScreens.LevelSelectionScreen);
            _levelSelectionScreen.ShowAsync(cancellationToken).Forget();
            _levelSelectionScreen.Initialize(_userDataService.GetUserData().UserProgressData.HighestLevelsUnlocked);
            _levelSelectionScreen.CreateSkinSelectionButtons(ConstGame.LevelCount);
        }

        private void Subscribe()
        {
            _levelSelectionScreen.StartLevelPressedSubject
                    .Subscribe(level =>
                    {
                        SetCurrentLevel(level);
                        GoTo<GameStateController>().Forget();
                    })
                    .AddTo(_cancellationTokenSource.Token);

            _levelSelectionScreen.LeavePressedSubject
                    .Subscribe(_ => GoTo<TitleStateController>().Forget())
                    .AddTo(_cancellationTokenSource.Token);

            _levelSelectionScreen.SettingsPressedSubject
                    .Subscribe(_ => _startSettingsController.RunIfNotRunning(_cancellationTokenSource.Token))
                    .AddTo(_cancellationTokenSource.Token);
        }

        private void SetCurrentLevel(int level) =>
                _userDataService.GetUserData().UserProgressData.SetCurrentLevel(level);
    }
}
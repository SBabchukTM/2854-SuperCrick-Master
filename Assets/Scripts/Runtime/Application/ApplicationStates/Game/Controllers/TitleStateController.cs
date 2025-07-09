using System.Threading;
using Application.UI;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using R3;
using Runtime.Application.ApplicationStates.Game.Controllers;
using Runtime.Core.Extensions;
using ILogger = Core.ILogger;

namespace Application.Game
{
    public class TitleStateController : StateController
    {
        private readonly StartSettingsController _startSettingsController;
        private readonly InfoPopupController _infoPopupController;
        private readonly ICleanupGame _cleanupGame;
        private readonly IChoosedTeamProvider _choosedTeamProvider;
        private readonly IUiService _uiService;

        private CancellationTokenSource _cancellationTokenSource;

        public TitleStateController(IUiService uiService, ILogger logger, StartSettingsController startSettingsController,
                InfoPopupController infoPopupController, ICleanupGame cleanupGame, IChoosedTeamProvider choosedTeamProvider) : base(logger)
        {
            _startSettingsController = startSettingsController;
            _infoPopupController = infoPopupController;
            _cleanupGame = cleanupGame;
            _choosedTeamProvider = choosedTeamProvider;
            _uiService = uiService;
        }

        public override async UniTask Enter(CancellationToken cancellationToken = default)
        {
            _cancellationTokenSource = new();
            _choosedTeamProvider?.Cleanup();
            await _cleanupGame.Cleanup(_cancellationTokenSource.Token);
            ShowTitleScreen(_cancellationTokenSource.Token);
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.TitleScreen);
            _cancellationTokenSource.Cancel();
        }

        private void ShowTitleScreen(CancellationToken cancellationToken)
        {
            var titleScreen = _uiService.GetScreen<TitleScreen>(ConstScreens.TitleScreen);
            Subscribe(titleScreen);

            titleScreen.ShowAsync(cancellationToken).Forget();
        }

        private void Subscribe(TitleScreen titleScreen)
        {
            SubscribeController<ChooseTeamState>(titleScreen.PlayButtonPressSubject);
            SubscribeController(titleScreen.InfoButtonPressSubject, _infoPopupController);
            SubscribeController(titleScreen.SettingsButtonPressSubject, _startSettingsController);
        }
        
        private void SubscribeController(Subject<Unit> subject, BaseController baseController) =>
                subject.Subscribe(_ => baseController.RunIfNotRunning(_cancellationTokenSource.Token))
                        .AddTo(_cancellationTokenSource.Token);

        private void SubscribeController<T>(Subject<Unit> subject) where T : StateController =>
                subject.Subscribe(_ => GoTo<T>().Forget())
                        .AddTo(_cancellationTokenSource.Token);
    }
}
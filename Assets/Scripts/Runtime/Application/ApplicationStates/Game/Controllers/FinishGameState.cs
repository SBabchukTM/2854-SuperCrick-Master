using System.Threading;
using Application.UI;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using R3;
using Runtime.Application.ApplicationStates.Game.Controllers;
using Runtime.Application.Services.Chart.Provider;
using Runtime.Core.Extensions;

namespace Application.Game
{
    public class FinishGameState : StateController
    {
        private readonly IUiService _uiService;
        private readonly IGameResultProvider _gameResultProvider;
        private readonly StartSettingsController _startSettingsController;
        private readonly IChartProvider _chartProvider;

        private FinishGamePopup _finishGamePopup;

        public FinishGameState(ILogger logger, IUiService uiService, IGameResultProvider gameResultProvider,
                StartSettingsController startSettingsController, IChartProvider chartProvider) : base(logger)
        {
            _uiService = uiService;
            _gameResultProvider = gameResultProvider;
            _startSettingsController = startSettingsController;
            _chartProvider = chartProvider;
        }

        public override UniTask Enter(CancellationToken cancellationToken = default)
        {
            _finishGamePopup = InitializeGameOverPopup(out var finishGamePopupData);
            _finishGamePopup.Show(finishGamePopupData, cancellationToken);

            Subscribe<InitGameController>(_finishGamePopup.PlayButtonPressSubject, cancellationToken);
            Subscribe(_finishGamePopup.SettingsButtonPressSubject, _startSettingsController, cancellationToken);
            Subscribe<TitleStateController>(_finishGamePopup.ExitButtonPressSubject, cancellationToken);
            
            return UniTask.CompletedTask;
        }

        public override UniTask Exit()
        {
            _finishGamePopup.DestroyPopup();

            return base.Exit();
        }

        private FinishGamePopup InitializeGameOverPopup(out FinishGamePopupData finishGamePopupData)
        {
            finishGamePopupData = new()
            {
                CenterSprite = _gameResultProvider.GetGameResultConfig().Sprite,
                Message = _gameResultProvider.GetGameResultConfig().Message,
                Score = _chartProvider.GetChartScore()
            };
            
            return _uiService.GetPopup<FinishGamePopup>(ConstPopups.FinishGamePopup);
        }

        private void Subscribe<T>(Subject<Unit> subject, CancellationToken cancellationToken) where T : StateController =>
                subject
                        .Subscribe(_ => GoTo<T>(cancellationToken).Forget())
                        .AddTo(cancellationToken);
        
        private static void Subscribe(Subject<Unit> subject, BaseController baseController, CancellationToken cancellationToken) =>
                subject.Subscribe(_ => baseController.RunIfNotRunning(cancellationToken))
                        .AddTo(cancellationToken);
    }
    
    
}
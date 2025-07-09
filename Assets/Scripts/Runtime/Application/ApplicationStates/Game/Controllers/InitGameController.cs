using System.Collections.Generic;
using System.Threading;
using Application.UI;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using R3;
using Runtime.Application.Services.Chart.Provider;
using Runtime.Core.Extensions;
using ILogger = Core.ILogger;

namespace Application.Game
{
    public class InitGameController : StateController
    {
        private readonly IUiService _uiService;
        private readonly ISetupGameState _setupGameState;
        private readonly ICleanupGame _cleanupGame;
        private readonly EmitPauseController _emitPauseController;
        private readonly List<IReset> _resetProviders;

        public InitGameController(ILogger logger, IUiService uiService, ISetupGameState setupGameState, ICleanupGame cleanupGame, EmitPauseController 
                emitPauseController, IChoosedTeamProvider choosedTeamProvider, IChartProvider chartProvider) : base(logger)
        {
            _uiService = uiService;
            _setupGameState = setupGameState;
            _cleanupGame = cleanupGame;
            _emitPauseController = emitPauseController;

            _resetProviders = new()
            {
                choosedTeamProvider,
                chartProvider
            };
        }
        
        public override async UniTask Enter(CancellationToken cancellationToken = default)
        {
            foreach (var resetProvider in _resetProviders)
                resetProvider?.Reset();
            
            await _cleanupGame.Cleanup(cancellationToken);
            await _setupGameState.Setup();
            
            var gameScreen = CreateScreen(cancellationToken);
            Subscribe(cancellationToken, gameScreen);

            GoTo<GameStateController>(cancellationToken).Forget();
        }

        private void Subscribe(CancellationToken cancellationToken, GameScreen gameScreen) =>
                gameScreen.PauseButtonPressSubject
                        .Subscribe(_ => _emitPauseController.RunIfNotRunning(cancellationToken))
                        .AddTo(cancellationToken);

        private GameScreen CreateScreen(CancellationToken cancellationToken)
        {
            var gameScreen = _uiService.GetScreen<GameScreen>(ConstScreens.GameScreen);
            gameScreen.ShowAsync(cancellationToken).Forget();
            return gameScreen;
        }
    }
}
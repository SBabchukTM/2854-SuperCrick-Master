using System.Threading;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Application.ApplicationStates.Game.Features.Bat;
using Runtime.Application.ApplicationStates.Game.Features.Enemy;
using Runtime.Core.Extensions;
using ILogger = Core.ILogger;

namespace Application.Game
{
    public class GameStateController : StateController
    {
        private readonly EmitPauseController _emitPauseController;
        private readonly BatLifeCycleController _batLifeCycleController;
        private readonly EnemyAttackController _enemyAttackController;
        private readonly GameResultController _gameResultController;

        private CancellationTokenSource _cancellationTokenSource;

        public GameStateController(ILogger logger, EmitPauseController emitPauseController, BatLifeCycleController batLifeCycleController,
                EnemyAttackController enemyAttackController, GameResultController gameResultController) : base(logger)
        {
            _emitPauseController = emitPauseController;
            _batLifeCycleController = batLifeCycleController;
            _enemyAttackController = enemyAttackController;
            _gameResultController = gameResultController;
        }

        public override async UniTask Enter(CancellationToken cancellationToken = default)
        {
            _cancellationTokenSource = new();

            await _batLifeCycleController.Run(_cancellationTokenSource.Token);
            await _enemyAttackController.Run(_cancellationTokenSource.Token);
            await _gameResultController.RunIfNotRunningAsync(_cancellationTokenSource.Token);
        }
        
        public override async UniTask Exit()
        {
            await _emitPauseController.StopIfRunningAsync();
            await _batLifeCycleController.StopIfRunningAsync();
            await _enemyAttackController.StopIfRunningAsync();
            await _gameResultController.StopIfRunningAsync();
            
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}
using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using R3;
using Runtime.Application.ApplicationStates.Game.Controllers;
using Runtime.Core.Extensions;

namespace Application.Game
{
    public class EmitPauseController : BaseController
    {
        private readonly PauseController _pauseController;
        private readonly StartSettingsController _startSettingsController;
        private readonly StateMachine _stateMachine;
        
        public EmitPauseController(PauseController pauseController, StartSettingsController startSettingsController, StateMachine stateMachine) 
        {
            _pauseController = pauseController;
            _startSettingsController = startSettingsController;
            _stateMachine = stateMachine;
        }

        public override UniTask Run(CancellationToken cancellationToken)
        {
            Subscribe(cancellationToken);

            _pauseController.RunIfNotRunning(cancellationToken);

            return base.Run(cancellationToken);
        }

        public override UniTask Stop()
        {
            StopControllers();

            return base.Stop();
        }

        private void Subscribe(CancellationToken cancellationToken)
        {
            SubscribeStateMachine<TitleStateController>(_pauseController.HomeButtonSubject, cancellationToken);
            SubscribeStateMachine<InitGameController>(_pauseController.RestartButtonsSubject, cancellationToken);
            
            _pauseController.ReturnToGameButtonSubject.Subscribe(_ => this.StopIfRunning()).AddTo(cancellationToken);
            _pauseController.SettingsButtonsSubject.Subscribe(_ => _startSettingsController.RunIfNotRunning(cancellationToken)).AddTo(cancellationToken);
        }

        private void SubscribeStateMachine<T>(Subject<Unit> subject, CancellationToken cancellationToken) where T : StateController =>
                subject.Subscribe(_ =>
                {
                    this.StopIfRunning();
                    _stateMachine?.GoTo<T>(cancellationToken).Forget();
                }).AddTo(cancellationToken);

        private void StopControllers()
        {
            _startSettingsController?.StopIfRunning();
            _pauseController?.StopIfRunning();
        }
    }
}
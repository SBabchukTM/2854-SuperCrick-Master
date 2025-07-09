using System.Threading;
using Application.UI;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using R3;

namespace Application.Game
{
    public class GameOverState : StateController
    {
        private readonly IUiService _uiService;

        public GameOverState(ILogger logger, IUiService uiService) : base(logger) =>
                _uiService = uiService;

        public override UniTask Enter(CancellationToken cancellationToken = default)
        {
            var gameOverPopup = InitializeGameOverPopup(out var simpleDecisionPopupData);
            gameOverPopup.Show(simpleDecisionPopupData, cancellationToken);
            Subscribe(cancellationToken, gameOverPopup);
            
            return UniTask.CompletedTask;
        }

        private void Subscribe(CancellationToken cancellationToken, SimpleDecisionPopup gameOverPopup) =>
                gameOverPopup.CloseButtonPressedSubject
                        .Subscribe(_ => GoTo<TitleStateController>(cancellationToken).Forget())
                        .AddTo(cancellationToken);

        private SimpleDecisionPopup InitializeGameOverPopup(out SimpleDecisionPopupData simpleDecisionPopupData)
        {
            simpleDecisionPopupData = new()
            {
                PressOkEvent = RestartGame
            };
            
            return _uiService.GetPopup<SimpleDecisionPopup>(ConstPopups.GameOverPopup);
        }

        private void RestartGame() =>
            GoTo<InitGameController>().Forget();
    }
}
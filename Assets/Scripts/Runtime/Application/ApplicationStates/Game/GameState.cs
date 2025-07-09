using System.Threading;
using Application.Game;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Application.ApplicationStates.Game.Controllers;

namespace Application.GameStateMachine
{
    public class GameState : StateController
    {
        private readonly StateMachine _stateMachine;

        private readonly ShopStateController _shopStateController;
        private readonly UserDataStateChangeController _userDataStateChangeController;
        private readonly TitleStateController _titleStateController;
        private readonly GameStateController _gameStateController;
        private readonly LevelSelectionController _levelSelectionController;
        private readonly InitShopState _initShopState;
        private readonly InitGameController _initGameController;
        private readonly ChooseTeamState _chooseTeamState;
        private readonly FinishGameState _finishGameState;

        public GameState(ILogger logger, TitleStateController titleStateController, StateMachine stateMachine,
            UserDataStateChangeController userDataStateChangeController, ShopStateController shopStateController,
            GameStateController gameStateController, LevelSelectionController levelSelectionController, InitShopState initShopState, 
            InitGameController initGameController, ChooseTeamState chooseTeamState, FinishGameState finishGameState) : base(logger)
        {
            _levelSelectionController = levelSelectionController;
            _initShopState = initShopState;
            _initGameController = initGameController;
            _chooseTeamState = chooseTeamState;
            _finishGameState = finishGameState;
            _gameStateController = gameStateController;
            _titleStateController = titleStateController;
            _stateMachine = stateMachine;
            _shopStateController = shopStateController;
            _userDataStateChangeController = userDataStateChangeController;
        }

        public override async UniTask Enter(CancellationToken cancellationToken = default)
        {
            await _userDataStateChangeController.Run(CancellationToken.None);
            _stateMachine.Initialize(_titleStateController, _gameStateController, _levelSelectionController, _initShopState, 
                    _shopStateController, _initGameController, _chooseTeamState, _finishGameState);
            _stateMachine.GoTo<TitleStateController>(cancellationToken).Forget();
        }
    }
}
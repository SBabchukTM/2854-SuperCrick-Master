using System.Threading;
using Application.Services.Audio;
using Core;
using Core.Services.Audio;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using R3;
using Runtime.Application.Services.Chart.Provider;

namespace Application.Game
{
    public class GameResultController : BaseController
    {
        private const float Volume = 0.6f;
        
        private readonly IChartProvider _chartProvider;
        private readonly IGameResultProvider _gameResultProvider;
        private readonly ISettingProvider _settingProvider;
        private readonly IBallLaunchProvider _ballLaunchProvider;
        private readonly StateMachine _stateMachine;
        private readonly IAudioService _audioService;

        private int _scoreToEnd;

        public GameResultController(IChartProvider chartProvider, IGameResultProvider gameResultProvider, ISettingProvider settingProvider, 
            IBallLaunchProvider ballLaunchProvider, StateMachine stateMachine, IAudioService audioService)
        {
            _chartProvider = chartProvider;
            _gameResultProvider = gameResultProvider;
            _settingProvider = settingProvider;
            _stateMachine = stateMachine;
            _audioService = audioService;
            _ballLaunchProvider = ballLaunchProvider;
        }

        public override UniTask Run(CancellationToken cancellationToken)
        {
            _scoreToEnd = _settingProvider.Get<GameConfig>().ScoreToWin;
            
            Subscribe(cancellationToken);

            return base.Run(cancellationToken);
        }

        private void Subscribe(CancellationToken cancellationToken)
        {
            _chartProvider.PlayerScoreSubject
                    .Subscribe(amount => CheckForEndGame(amount, TeamTypeId.Player))
                    .AddTo(cancellationToken);

            _chartProvider.EnemyScoreSubject
                    .Subscribe(amount => CheckForEndGame(amount, TeamTypeId.Enemy))
                    .AddTo(cancellationToken);
        }

        private void CheckForEndGame(int amount, TeamTypeId teamTypeId)
        {
            if(amount >= _scoreToEnd)
            {
                _gameResultProvider.SetGameResult(GetGameResult(teamTypeId));

                PlaySound();
                
                _stateMachine.GoTo<FinishGameState>().Forget();
            }

            if(amount > 0)
                _ballLaunchProvider?.Launch();
        }

        private void PlaySound()
        {
            switch(_gameResultProvider.GetGameResult())
            {
                case GameResult.Lose:
                    _audioService.PlaySound(ConstAudio.LoseSound, Volume);
                    return;
                case GameResult.Win:
                    _audioService.PlaySound(ConstAudio.WinSound, Volume);
                    return;
            }
        }

        private static GameResult GetGameResult(TeamTypeId teamTypeId) =>
                teamTypeId switch
                {
                    TeamTypeId.Player => GameResult.Win,
                    TeamTypeId.Enemy => GameResult.Lose,
                    _ => GameResult.None
                };
    }
}
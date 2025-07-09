using System.Threading;
using Application.Game;
using Application.Services.Audio;
using Application.Services.CameraProvider;
using Code.Gameplay.Common.Random;
using Core;
using Core.Services.Audio;
using Cysharp.Threading.Tasks;
using ImprovedTimers;
using R3;
using Runtime.Application.ApplicationStates.Game.Features.Bat;
using Runtime.Application.ApplicationStates.Game.Features.Enemy.Factory;
using SRF;
using UnityEngine;
using Zenject;

namespace Runtime.Application.ApplicationStates.Game.Features.Enemy
{
    public class EnemyAttackController : BaseController, ITickable
    {
        private readonly IEnemyFactory _enemyFactory;
        private readonly ISettingProvider _settingProvider;
        private readonly IBallFactory _ballFactory;
        private readonly ILevelPointProvider _levelPointProvider;
        private readonly IRandomService _randomService;
        private readonly ICameraProvider _cameraProvider;
        private readonly IChoosedTeamProvider _choosedTeamProvider;
        private readonly IBallLaunchProvider _ballLaunchProvider;
        private readonly IAudioService _audioService;

        private Enemy _enemy;
        private BallConfig _ballConfig;
        private EnemyConfig _enemyConfig;
        private CountdownTimer _countdownTimer;
        private Ball _ball;

        public EnemyAttackController(IEnemyFactory enemyFactory, ISettingProvider settingProvider, IBallFactory ballFactory,
                ILevelPointProvider levelPointProvider, IRandomService randomService, ICameraProvider cameraProvider, IChoosedTeamProvider choosedTeamProvider, 
                IBallLaunchProvider ballLaunchProvider, IAudioService audioService)
        {
            _enemyFactory = enemyFactory;
            _settingProvider = settingProvider;
            _ballFactory = ballFactory;
            _levelPointProvider = levelPointProvider;
            _randomService = randomService;
            _cameraProvider = cameraProvider;
            _choosedTeamProvider = choosedTeamProvider;
            _ballLaunchProvider = ballLaunchProvider;
            _audioService = audioService;
        }

        public override async UniTask Run(CancellationToken cancellationToken)
        {
            SetConfigs();
            CreateEnemy();
            SubscribeProvider(cancellationToken);
            Launch(cancellationToken);

            await base.Run(cancellationToken);
        }

        public void Tick()
        {
            if(CurrentState == ControllerState.Run && _countdownTimer != null)
                _countdownTimer.Tick();
        }

        public override UniTask Stop()
        {
            if(_enemy)
                Object.Destroy(_enemy.gameObject);

            _countdownTimer?.Dispose();
            _countdownTimer = null;
            
            return base.Stop();
        }

        private void SubscribeProvider(CancellationToken cancellationToken) =>
                _ballLaunchProvider.ReadyToLaunchSubject.Subscribe(_ => SetLaunchTimer(cancellationToken)).AddTo(cancellationToken);

        private void CreateEnemy() =>
                _enemy = _enemyFactory.Create(_levelPointProvider.GetPoint(LevelPointTypeId.EnemyPosition), _enemyConfig.View);

        private void SetLaunchTimer(CancellationToken cancellationToken)
        {
            _countdownTimer = new(_enemyConfig.TimeToCreateBall);
            _countdownTimer.Start();
            _countdownTimer.OnTimerStop += () =>  Launch(cancellationToken);
        }

        private void Launch(CancellationToken cancellationToken)
        {
            if(cancellationToken.IsCancellationRequested)
                return;
            
            if(_ball)
                return;

            _ball = Create(cancellationToken);
            _audioService.PlaySound(ConstAudio.LaunchSound, 0.3f);
            InitializeMoveDirection(_ball);
            Move(cancellationToken, _ball);
            Subscribe(_ball);
        }

        private void Subscribe(Ball ball) =>
                ball.BallDestroyedSubject.Subscribe(_ => AddScore()).AddTo(ball);

        private static void Move(CancellationToken cancellationToken, Ball ball) =>
                ball.MoveAsync(cancellationToken).Forget();

        private void InitializeMoveDirection(Ball ball)
        {
            var direction = _randomService.GetRandomDirection(_enemy.transform.position);
            var travelDistance = CalculateTravelDistance(ball.transform.position, direction);
            ball.Initialize(direction, _ballConfig.Speed, travelDistance, _ballConfig.MaxScale);
        }

        private Ball Create(CancellationToken cancellationToken) =>
                cancellationToken.IsCancellationRequested ? null : _ballFactory.Create(_enemy.BallSpawnPosition.position, _ballConfig.View, _ballConfig.Sprites.Random());

        private void SetConfigs()
        {
            _enemyConfig = _settingProvider.Get<EnemyConfig>();
            _ballConfig = _settingProvider.Get<BallConfig>();
        }

        private void AddScore() =>
                _choosedTeamProvider.AddScore(_choosedTeamProvider.GetTeam(TeamTypeId.Enemy));

        private float CalculateTravelDistance(Vector3 startPosition, Vector2 direction)
        {
            var bottomLeft = _cameraProvider.MainCamera.ViewportToWorldPoint(new(0, 0.5f, _cameraProvider.MainCamera.nearClipPlane));
            var topRight = _cameraProvider.MainCamera.ViewportToWorldPoint(new(1, 0.5f, _cameraProvider.MainCamera.nearClipPlane));

            var distanceX = direction.x > 0 ? (topRight.x - startPosition.x) / direction.x : (bottomLeft.x - startPosition.x) / direction.x;

            return Mathf.Abs(distanceX);
        }
    }
}
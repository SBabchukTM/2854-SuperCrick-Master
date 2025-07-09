using System.Threading;
using Application.Game;
using Core;
using Cysharp.Threading.Tasks;
using R3;
using Runtime.Application.ApplicationStates.Game.Features.Bat.Factory;
using Runtime.Application.ApplicationStates.Game.Features.Bat.Runtime.Application.ApplicationStates.Game.Features.Bat;
using SRF;
using UnityEngine;

namespace Runtime.Application.ApplicationStates.Game.Features.Bat
{
    public class BatLifeCycleController : BaseController
    {
        private readonly IBatLauncherFactory _batLauncherFactory;
        private readonly ISettingProvider _settingProvider;
        private readonly IBallScaleProvider _ballScaleProvider;
        private readonly IChoosedTeamProvider _choosedTeamProvider;

        private BatController _batLauncher;

        public BatLifeCycleController(IBatLauncherFactory batLauncherFactory, ISettingProvider settingProvider, IBallScaleProvider 
                ballScaleProvider, IChoosedTeamProvider choosedTeamProvider)
        {
            _batLauncherFactory = batLauncherFactory;
            _settingProvider = settingProvider;
            _ballScaleProvider = ballScaleProvider;
            _choosedTeamProvider = choosedTeamProvider;
        }

        public override async UniTask Run(CancellationToken cancellationToken)
        {
            var position = new Vector2(0.5f, -1f);
            var batConfig = _settingProvider.Get<BatConfig>();

            _batLauncher = _batLauncherFactory.Create(position, batConfig.View, batConfig.Sprites.Random());
            _batLauncher.Restore();

            _ballScaleProvider.Scale
                    .Subscribe(_batLauncher.UpdateBallScale)
                    .AddTo(cancellationToken);

            _batLauncher.HitBallSubject
                    .Subscribe(_ => OnHitBall())
                    .AddTo(cancellationToken);

            await base.Run(cancellationToken);
        }

        public override UniTask Stop()
        {
            if(_batLauncher)
                Object.Destroy(_batLauncher.gameObject);
            
            return base.Stop();
        }

        private void OnHitBall() =>
                _choosedTeamProvider.AddScore(_choosedTeamProvider.GetTeam(TeamTypeId.Player));
    }
}
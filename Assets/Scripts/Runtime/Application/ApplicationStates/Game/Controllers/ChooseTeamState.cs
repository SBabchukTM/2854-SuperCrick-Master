using System.Collections.Generic;
using System.Threading;
using Application.UI;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using R3;
using SRF;
using UnityEngine;
using ILogger = Core.ILogger;

namespace Application.Game
{
    public class ChooseTeamState : StateController
    {
        private readonly IUiService _uiService;
        private readonly ISettingProvider _settingProvider;

        private ChooseTeamScreen _chooseTeamScreen;

        public ChooseTeamState(ILogger logger, IUiService uiService, ISettingProvider settingProvider) : base(logger)
        {
            _uiService = uiService;
            _settingProvider = settingProvider;
        }

        public override UniTask Enter(CancellationToken cancellationToken = default)
        {
            var chooseTeamPopup = Create(cancellationToken);
            
            SubscribeController<InitGameController>(chooseTeamPopup.PlayButtonPressedSubject, cancellationToken);
            SubscribeController<TitleStateController>(chooseTeamPopup.CloseButtonPressedSubject, cancellationToken);

            return UniTask.CompletedTask;
        }

        public override async UniTask Exit() =>
                await _uiService.HideScreen(ConstScreens.ChooseTeamScreen);

        private ChooseTeamScreen Create(CancellationToken cancellationToken)
        {
            _chooseTeamScreen = _uiService.GetScreen<ChooseTeamScreen>(ConstScreens.ChooseTeamScreen);
            var teamConfig = _settingProvider.Get<TeamConfig>();
            var teamConfigCopySprites = new List<Sprite>(teamConfig.Sprites);

            _chooseTeamScreen.ShowAsync(cancellationToken).Forget();
            _chooseTeamScreen.CreateTeamButtons(teamConfig.View, GetRandomSprite(teamConfigCopySprites), GetRandomSprite(teamConfigCopySprites));

            return _chooseTeamScreen;
        }

        private static Sprite GetRandomSprite(List<Sprite> sprites)
        {
            var randomSprite = sprites.Random();
            sprites.Remove(randomSprite);

            return randomSprite;
        }
        
        private void SubscribeController<T>(Subject<Unit> subject, CancellationToken cancellationToken) where T : StateController =>
                subject.Subscribe(_ => GoTo<T>(cancellationToken).Forget())
                        .AddTo(cancellationToken);
    }
}
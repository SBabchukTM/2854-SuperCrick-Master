using System;
using System.Threading;
using Application.Game;
using Application.Services.UserData;
using Application.UI;
using Core;
using Core.Services.ScreenOrientation;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ILogger = Core.ILogger;

namespace Application.GameStateMachine
{
    public class BootstrapState : StateController, IDisposable
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IUiService _uiService;
        private readonly ISettingProvider _settingProvider;
        private readonly UserDataService _userDataService;
        private readonly AudioSettingsBootstrapController _audioSettingsBootstrapController;
        private readonly ScreenOrientationAlertController _screenOrientationAlertController;
        private readonly IFinishedComponentsService _finishedComponentsService;
        private readonly LevelProgressController _levelProgressController;


        public BootstrapState(IAssetProvider assetProvider,
            IUiService uiService,
            ILogger logger,
            ISettingProvider settingProvider,
            UserDataService userDataService,
            AudioSettingsBootstrapController audioSettingsBootstrapController,
            ScreenOrientationAlertController screenOrientationAlertController,
            IFinishedComponentsService finishedComponentsService,
            LevelProgressController levelProgressController) : base(logger)
        {
            _uiService = uiService;
            _assetProvider = assetProvider;
            _uiService = uiService;
            _settingProvider = settingProvider;
            _userDataService = userDataService;
            _audioSettingsBootstrapController = audioSettingsBootstrapController;
            _screenOrientationAlertController = screenOrientationAlertController;

            _finishedComponentsService = finishedComponentsService;
            _levelProgressController = levelProgressController;
            _finishedComponentsService.RegisterIFinished(_levelProgressController);
        }

        public override async UniTask Enter(CancellationToken cancellationToken = default)
        {
            Input.multiTouchEnabled = false;

            _userDataService.Initialize();
            await _assetProvider.Initialize();
            await _uiService.Initialize();
            await _settingProvider.Initialize();
            await _screenOrientationAlertController.Run(CancellationToken.None);
            _uiService.ShowScreen(ConstScreens.SplashScreen, cancellationToken).Forget();
            await _audioSettingsBootstrapController.Run(CancellationToken.None);
            UpdateSession();

            GoTo<GameState>(cancellationToken).Forget();
        }

        public override async UniTask Exit() => await _uiService.HideScreen(ConstScreens.SplashScreen);

        private void UpdateSession()
        {
            _userDataService.GetUserData().GameData.SessionNumber++;
            _userDataService.SaveUserData();
        }

        public void Dispose()
        {
            _finishedComponentsService.UnregisterIFinished(_levelProgressController);
        }
    }
}
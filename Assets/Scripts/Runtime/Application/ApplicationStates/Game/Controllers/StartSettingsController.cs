using System.Threading;
using Application.Game;
using Application.UI;
using Core;
using Core.Services.Audio;
using Cysharp.Threading.Tasks;
using R3;
using Runtime.Core.Extensions;

namespace Runtime.Application.ApplicationStates.Game.Controllers
{
    public sealed class StartSettingsController : BaseController
    {
        private readonly IUiService _uiService;
        private readonly ISettingsController _settingsController;

        private SoundSettingsPopup _settingsPopup;
        private CompositeDisposable _compositeDisposable;

        public StartSettingsController(IUiService uiService, ISettingsController settingsController)
        {
            _settingsController = settingsController;
            _uiService = uiService;
        }

        public override UniTask Run(CancellationToken cancellationToken)
        {
            _compositeDisposable = new();

            CreateSettingsPopup(cancellationToken);
            Subscribe();

            return base.Run(cancellationToken);
        }

        public override UniTask Stop()
        {
            _compositeDisposable.Dispose();

            return base.Stop();
        }

        private void CreateSettingsPopup(CancellationToken cancellationToken)
        {
            _settingsPopup = _uiService.GetPopup<SoundSettingsPopup>(ConstPopups.SoundSettingsPopup);

            _settingsPopup.Show(new SettingsPopupData(_settingsController.GetSoundVolume(), _settingsController.GetMusicVolume()),
                    cancellationToken).Forget();
        }

        private void Subscribe()
        {
            _settingsPopup.SoundVolumeChangeCommand
                .Subscribe(audio  => _settingsController.OnChangeVolume(AudioType.Sound, audio))
                .AddTo(_compositeDisposable);
            
            _settingsPopup.MusicVolumeChangeCommand
                .Subscribe(audio => _settingsController.OnChangeVolume(AudioType.Music, audio))
                .AddTo(_compositeDisposable);
            
            _settingsPopup.CloseButtonPressedSubject
                    .Subscribe(_ => this.StopIfRunning())
                    .AddTo(_compositeDisposable);
        }
    }
}
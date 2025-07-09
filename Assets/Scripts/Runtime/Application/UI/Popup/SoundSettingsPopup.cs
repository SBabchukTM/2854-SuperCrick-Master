using System.Threading;
using Application.Services.Audio;
using Application.UI;
using Core.UI;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using AudioType = Core.Services.Audio.AudioType;

namespace Application.Game
{
    public class SoundSettingsPopup : ClosablePopup
    {
        public readonly ReactiveCommand<bool> SoundVolumeChangeCommand = new();
        public readonly ReactiveCommand<bool> MusicVolumeChangeCommand = new();

        [SerializeField] private Toggle _soundVolumeToggle;
        [SerializeField] private Toggle _musicVolumeToggle;

        private SettingsPopupSpritesStorage _settingsPopupSpritesStorage;

        [Inject]
        public void Construct(SettingsPopupSpritesStorage settingsPopupSpritesStorage) =>
                _settingsPopupSpritesStorage = settingsPopupSpritesStorage;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            var settingsPopupData = data as SettingsPopupData;

            SubscribeToggle(_soundVolumeToggle, SoundVolumeChangeCommand, AudioType.Sound);
            SubscribeToggle(_musicVolumeToggle, MusicVolumeChangeCommand, AudioType.Music);
            InitializeVolumeToggle(_soundVolumeToggle, settingsPopupData.IsSoundVolume);
            InitializeVolumeToggle(_musicVolumeToggle, settingsPopupData.IsMusicVolume);

            return base.Show(data, cancellationToken);
        }

        private void SubscribeToggle(Toggle toggle, ReactiveCommand<bool> volumeChangeCommand, AudioType audioType) =>
                toggle.onValueChanged.AsObservable()
                        .Subscribe(value => OnVolumeToggleValueChanged(toggle, volumeChangeCommand, audioType, value))
                        .AddTo(this);

        private static void InitializeVolumeToggle(Toggle toggle, bool volume)
        {
            toggle.onValueChanged.Invoke(volume);
            toggle.isOn = volume;
        }

        private void OnVolumeToggleValueChanged(Toggle toggle, ReactiveCommand<bool> volumeChangeCommand, AudioType audioType, bool value)
        {
            toggle.ChangeToggleSprite(toggle.GetToggleVisuals(_settingsPopupSpritesStorage, audioType));

            AudioService.PlaySound(ConstAudio.PressButtonSound);

            volumeChangeCommand?.Execute(value);
        }
    }
}
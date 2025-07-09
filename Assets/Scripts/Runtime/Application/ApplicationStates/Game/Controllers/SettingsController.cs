using System;
using Application.Services.UserData;
using Core.Services.Audio;

namespace Application.Game
{
    public class SettingsController : ISettingsController
    {
        private readonly SettingsData _settingsData;
        private readonly UserDataService _userDataService;
        private readonly IAudioService _audioService;

        public SettingsController(UserDataService userDataService, IAudioService audioService)
        {
            _userDataService = userDataService;
            _audioService = audioService;
        }

        public void OnChangeVolume(AudioType audioType, bool volume)
        {
            SetVolumeData(audioType, volume);
            SetAudioVolume(audioType, volume ? 1 : 0);
        }

        public bool GetMusicVolume() => _userDataService.GetUserData().SettingsData.IsMusicVolume;

        public bool GetSoundVolume() => _userDataService.GetUserData().SettingsData.IsSoundVolume;

        private void SetAudioVolume(AudioType audioType, int volume) => _audioService.SetVolume(audioType, volume);

        private void SetVolumeData(AudioType audioType, bool volume)
        {
            switch (audioType)
            {
                case AudioType.Music:
                    _userDataService.GetUserData().SettingsData.IsMusicVolume = volume;
                    break;
                case AudioType.Sound:
                    _userDataService.GetUserData().SettingsData.IsSoundVolume = volume;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(audioType), audioType, null);
            }
        }
    }
}
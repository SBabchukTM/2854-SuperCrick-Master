using Core.Services.Audio;

namespace Application.Game
{
    public interface ISettingsController
    {
        void OnChangeVolume(AudioType audioType, bool volume);

        bool GetMusicVolume();

        bool GetSoundVolume();
    }
}
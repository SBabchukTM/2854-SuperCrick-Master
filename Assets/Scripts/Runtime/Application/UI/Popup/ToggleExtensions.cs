using System;
using UnityEngine;
using UnityEngine.UI;
using AudioType = Core.Services.Audio.AudioType;

namespace Application.UI
{
    public static class ToggleExtensions
    {
public static Sprite GetToggleVisuals(this Toggle toggle, SettingsPopupSpritesStorage settingsPopupSpritesStorage, AudioType audioType) =>
                _ = audioType switch
                {
                    AudioType.Music => toggle.isOn ? settingsPopupSpritesStorage.MusicActiveSprite
                            : settingsPopupSpritesStorage.MusicDisactiveSprite,
                    AudioType.Sound => toggle.isOn ? settingsPopupSpritesStorage.SoundActiveSprite
                            : settingsPopupSpritesStorage.SoundDisactiveSprite,
                    _ => throw new ArgumentOutOfRangeException(nameof(audioType), audioType, null)
                };

        public static void ChangeToggleSprite(this Toggle toggle, Sprite sprite) =>
                toggle.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
    }
}
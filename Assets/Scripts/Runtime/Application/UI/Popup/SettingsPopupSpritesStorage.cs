using UnityEngine;

namespace Application.UI
{
    public record SettingsPopupSpritesStorage(
            Sprite MusicActiveSprite,
            Sprite MusicDisactiveSprite,
            Sprite SoundActiveSprite,
            Sprite SoundDisactiveSprite);
}
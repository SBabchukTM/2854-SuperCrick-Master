using R3;
using Runtime.Core.Extensions;
using UnityEngine;

namespace Application.UI
{
    public class TitleScreen : UiScreen
    {
        public readonly Subject<Unit> PlayButtonPressSubject = new();
        public readonly Subject<Unit> SettingsButtonPressSubject = new();
        public readonly Subject<Unit> InfoButtonPressSubject = new();

        [SerializeField] private SimpleButton _playButton;
        [SerializeField] private SimpleButton _settingsButton;
        [SerializeField] private SimpleButton _infoButton;
        [SerializeField] private SimpleButton _shopButton;

        private void OnEnable()
        {
            _playButton.Button.SubscribeButtonClick(PlayButtonPressSubject);
            _settingsButton.Button.SubscribeButtonClick(SettingsButtonPressSubject);
            _infoButton.Button.SubscribeButtonClick(InfoButtonPressSubject);
        }
    }
}
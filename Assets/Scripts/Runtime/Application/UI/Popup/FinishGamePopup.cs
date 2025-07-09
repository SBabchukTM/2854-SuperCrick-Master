using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using R3;
using Runtime.Core.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Application.UI
{
    public class FinishGamePopup : BasePopup
    {
        public readonly Subject<Unit> PlayButtonPressSubject = new();
        public readonly Subject<Unit> ExitButtonPressSubject = new();
        public readonly Subject<Unit> SettingsButtonPressSubject = new();

        [SerializeField] private SimpleButton _playButton;
        [SerializeField] private SimpleButton _exitButton;
        [SerializeField] private SimpleButton _settingsButton;
        
        [SerializeField] private Image _centerImage;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _finishPopupState;
        
        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            var finishGamePopupData = data as FinishGamePopupData;
            
            UpdateUI(finishGamePopupData);
            SubscribeButtons();
            
            return base.Show(data, cancellationToken);
        }

        private void UpdateUI(FinishGamePopupData finishGamePopupData)
        {
            _centerImage.sprite = finishGamePopupData.CenterSprite;
            _scoreText.text = finishGamePopupData.Score;
            _finishPopupState.text = finishGamePopupData.Message;
        }

        private void SubscribeButtons()
        {
            _playButton.Button.SubscribeButtonClick(PlayButtonPressSubject);
            _exitButton.SubscribeButtonClick(ExitButtonPressSubject);
            _settingsButton.SubscribeButtonClick(SettingsButtonPressSubject);
        }
    }
}
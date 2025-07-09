using R3;
using Runtime.Core.Extensions;
using UnityEngine;

namespace Application.UI
{
    public class GameScreen : UiScreen
    {
        public readonly Subject<Unit> PauseButtonPressSubject = new();

        [SerializeField] private SimpleButton _pauseButton;

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe() =>
                _pauseButton.SubscribeButtonClick(PauseButtonPressSubject);
    }
}
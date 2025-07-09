using System;
using System.Threading;
using Application.Game;
using Core.UI;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Application.UI
{
    public class PausePopup : ClosablePopup
    {
        public readonly Subject<Unit> OpenMenuSubject = new();
        public readonly Subject<Unit> RestartLevelSubject = new();
        public readonly Subject<Unit> SettingsButtonPressSubject = new();

        [SerializeField] private SimpleButton _restartLevelButton;
        [SerializeField] private SimpleButton _menuButton;
        [SerializeField] private SimpleButton _settingsButton;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            SubscribeEvents();
            return base.Show(data, cancellationToken);
        }

        private void SubscribeEvents()
        {
            SubscribeButtonAsObservable(_restartLevelButton.Button, RestartLevelSubject, DestroyPopup);
            SubscribeButtonAsObservable(_menuButton.Button, OpenMenuSubject, DestroyPopup);
            SubscribeButtonAsObservable(_settingsButton.Button, SettingsButtonPressSubject);

            gameObject.GetComponent<Image>()
                    .OnPointerClickAsObservable()
                    .Where(eventData => !IsClickOverUI(eventData.position, _restartLevelButton.Button) && 
                                        !IsClickOverUI(eventData.position, _menuButton.Button))
                    .Subscribe(_ => OnButtonPressed(CloseButtonPressedSubject, DestroyPopup))
                    .AddTo(this);
        }

        private void SubscribeButtonAsObservable(Button button, Subject<Unit> subject, Action action = null) =>
                button.OnClickAsObservable()
                        .Subscribe(_ => OnButtonPressed(subject, action))
                        .AddTo(this);

        private static bool IsClickOverUI(Vector2 position, Button button) =>
                RectTransformUtility.RectangleContainsScreenPoint(button.GetComponent<RectTransform>(), position, null);

        private static void OnButtonPressed(Subject<Unit> subject, Action action = null)
        {
            subject?.OnNext(Unit.Default);
            action?.Invoke();
        }
    }
}
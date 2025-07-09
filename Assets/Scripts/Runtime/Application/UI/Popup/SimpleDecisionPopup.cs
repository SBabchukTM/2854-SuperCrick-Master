using System.Threading;
using Application.Game;
using Core.UI;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Application.UI
{
    public class SimpleDecisionPopup : ClosablePopup
    {
        [SerializeField] private SimpleButton _confirmButton;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            SubscribeButton(data as SimpleDecisionPopupData);
            
            return base.Show(data, cancellationToken);
        }

        private void SubscribeButton(SimpleDecisionPopupData simpleDecisionPopupData) =>
                _confirmButton.Button
                        .OnClickAsObservable()
                        .Subscribe(_ => OnConfirmButtonPressed(simpleDecisionPopupData))
                        .AddTo(this);

        private void OnConfirmButtonPressed(SimpleDecisionPopupData simpleDecisionPopupData)
        {
            simpleDecisionPopupData.PressOkEvent?.Invoke();
            DestroyPopup();
        }
    }
}
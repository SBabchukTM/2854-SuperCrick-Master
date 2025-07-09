using System.Threading;
using Application.UI;
using Core.UI;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Application.Game
{
    public class ClosablePopup : BasePopup
    {
        public readonly Subject<Unit> CloseButtonPressedSubject = new();
        
        [SerializeField] private SimpleButton _closeButton;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _closeButton.Button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        CloseButtonPressedSubject.OnNext(Unit.Default);
                        DestroyPopup();
                    })
                    .AddTo(this);

            return base.Show(data, cancellationToken);
        }

    }
}
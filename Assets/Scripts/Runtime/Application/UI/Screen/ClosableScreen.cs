using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Application.UI
{
    public class ClosableScreen : UiScreen
    {
        public readonly Subject<Unit> CloseButtonPressedSubject = new();
        
        [SerializeField] private SimpleButton _closeButton;
        
        public override UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            _closeButton.Button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        CloseButtonPressedSubject.OnNext(Unit.Default);
                        DestroyScreen();
                    })
                    .AddTo(this);

            return base.ShowAsync(cancellationToken);
        }
    }
}
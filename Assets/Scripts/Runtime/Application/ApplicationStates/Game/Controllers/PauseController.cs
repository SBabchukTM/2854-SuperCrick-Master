using System.Threading;
using Application.UI;
using Core;
using Cysharp.Threading.Tasks;
using R3;

namespace Application.Game
{
    public class PauseController : BaseController
    {
        private readonly IUiService _uiService;

        public readonly Subject<Unit> RestartButtonsSubject = new();
        public readonly Subject<Unit> HomeButtonSubject = new();
        public readonly Subject<Unit> ReturnToGameButtonSubject = new();
        public readonly Subject<Unit> SettingsButtonsSubject = new();

        private PausePopup _pausePopup;
        private CompositeDisposable _compositeDisposable;

        public PauseController(IUiService uiService) =>
                _uiService = uiService;

        public override UniTask Run(CancellationToken cancellationToken)
        {
            _compositeDisposable = new();

            Show(cancellationToken);
            Subscribe();
            TakePause(GameStates.PausedState);

            return base.Run(cancellationToken);
        }

        public override UniTask Stop()
        {
            _compositeDisposable?.Dispose();

            TakePause(GameStates.RunningState);

            return base.Stop();
        }

        private static void TakePause(GameStates gameStates) =>
                PauseTaker.TakePause((int)gameStates);

        private void Show(CancellationToken cancellationToken)
        {
            _pausePopup = _uiService.GetPopup<PausePopup>(ConstPopups.PausePopup);

            _pausePopup.Show(null, cancellationToken);
        }

        private void Subscribe()
        {
            _pausePopup.OpenMenuSubject.Subscribe(_ => OnButtonPressed(HomeButtonSubject)).AddTo(_compositeDisposable);
            _pausePopup.RestartLevelSubject.Subscribe(_ => OnButtonPressed(RestartButtonsSubject)).AddTo(_compositeDisposable);
            _pausePopup.SettingsButtonPressSubject.Subscribe(_ => OnButtonPressed(SettingsButtonsSubject)).AddTo(_compositeDisposable);
            _pausePopup.CloseButtonPressedSubject.Subscribe(_ => OnButtonPressed(ReturnToGameButtonSubject)).AddTo(_compositeDisposable);
        }

        private static void OnButtonPressed(Subject<Unit> subject) =>
            subject?.OnNext(Unit.Default);
    }
}
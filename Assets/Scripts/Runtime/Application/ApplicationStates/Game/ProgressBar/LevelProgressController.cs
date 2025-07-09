using System;
using System.Threading;
using Application.Services.UserData;
using Core;
using Cysharp.Threading.Tasks;
using ImprovedTimers;
using R3;
using Zenject;

namespace Application.Game
{
    public class LevelProgressController : BaseController, ITickable, IFinished
    {
        private readonly UserDataService _userDataService;

        private ProgressBar _progressBar;
        private CountdownTimer _countdownTimer;
        private IDisposable _disposable;

        public LevelProgressController(UserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        public void Tick()
        {
            if(CurrentState != ControllerState.Run)
                return;

            if(_countdownTimer is { IsRunning: true })
                UpdateLevelProgress((int)_countdownTimer.Progress);
        }

        public override UniTask Run(CancellationToken cancellationToken)
        {
            ResetProgress();
            SetupTimer();

            return base.Run(cancellationToken);
        }

        public override UniTask Stop()
        {
            _disposable.Dispose();
            _countdownTimer.Stop();
            _countdownTimer.Dispose();
            Unsubscribe();

            return base.Stop();
        }

        public void SetProgressBar(ProgressBar progressBar)
        {
            _progressBar = progressBar;

            Subscribe();
        }

        public void Finish() => ResetProgress();

        private void SetupTimer()
        {
            var maxLevelProgress = 100 * _userDataService.GetUserData().UserProgressData.CurrentLevel;
            _countdownTimer = new(maxLevelProgress);
            _countdownTimer.Start();
        }

        private void UpdateLevelProgress(int level) => _progressBar.BarValue = level;

        private void ResetProgress()
        {
            ResetTimer();
            ResetBar();

            _progressBar.SetBarMaxValue(5);
        }

        private void ResetBar() => _progressBar.ResetBar();

        private void ResetTimer()
        {
            _countdownTimer?.Reset();
            _countdownTimer?.Pause();
        }

        private void Subscribe() => _disposable = _progressBar.LevelFinishedSubject.Subscribe(_ => OnLevelFinished());

        private void Unsubscribe() => _disposable.Dispose();

        private void OnLevelFinished()
        {
            _countdownTimer.Reset();
            _userDataService.GetUserData().UserProgressData.CurrentLevel++;
        }
    }
}
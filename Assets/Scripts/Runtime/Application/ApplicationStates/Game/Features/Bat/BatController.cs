using Application.Services.Audio;
using Core;
using Core.Services.Audio;
using R3;
using UnityEngine;
using Zenject;

namespace Runtime.Application.ApplicationStates.Game.Features.Bat.Runtime.Application.ApplicationStates.Game.Features.Bat
{
    public class BatController : MonoBehaviour
    {
        public Subject<Unit> HitBallSubject = new();
        
        [SerializeField] private BatView _batView;
        
        private ISettingProvider _settingProvider;
        private IAudioService _audioService;

        [Inject]
        public void Construct(ISettingProvider settingProvider, IAudioService audioService)
        {
            _audioService = audioService;
            _settingProvider = settingProvider;
        }

        private void Start()
        {
            _batView.HitBallSubject.Subscribe(_ => OnHitBall()).AddTo(this);
            _batView.HitSoundSubject.Subscribe(_ => OnPlayedHitSound()).AddTo(this);
        }

        public void Restore() =>
                HitBallSubject = new();

        public void SetBatSprite(Sprite sprite) =>
                _batView.SetSprite(sprite);

        public void UpdateBallScale(float scale)
        {
            if(scale >= _settingProvider.Get<BallConfig>().MaxScale && HitBallSubject.IsDisposed == false)
                _batView.IsReadyToProtect = true;
        }

        private void OnHitBall()
        {
            _batView.IsReadyToProtect = false;
            HitBallSubject?.OnNext(Unit.Default);
        }

        private void OnPlayedHitSound() =>
                _audioService.PlaySound(ConstAudio.BeatBallSound, 0.4f);
    }
}
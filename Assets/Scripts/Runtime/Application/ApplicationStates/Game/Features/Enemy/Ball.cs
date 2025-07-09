using System.Threading;
using Application.Game;
using Application.Services.CameraProvider;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ImprovedTimers;
using R3;
using Runtime.Application.ApplicationStates.Game.Features.Bat;
using UnityEngine;
using Zenject;

namespace Runtime.Application.ApplicationStates.Game.Features.Enemy
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Ball : Destroyable
    {
        private const float MaxDistance = 1.3f;
        private const float MinDistance = 0.3f;

        public readonly Subject<Unit> BallDestroyedSubject = new();
        
        private IBallScaleProvider _ballScaleProvider;
        private ICameraProvider _cameraProvider;
        private ITimeService _timeService;
        private SpriteRenderer _spriteRenderer;
        private Vector2 _direction;
        private Vector2 _startPosition;
        private float _speed;
        private float _travelDistance;
        private float _initialScale;
        private float _maxScale;
        private bool _destructed;

        [Inject]
        public void Construct(IBallScaleProvider ballScaleProvider, ICameraProvider cameraProvider, ITimeService timeService)
        {
            _timeService = timeService;
            _cameraProvider = cameraProvider;
            _ballScaleProvider = ballScaleProvider;
        }

        private void Awake() =>
                _spriteRenderer = GetComponent<SpriteRenderer>();

        public void Initialize(Vector2 direction, float ballConfigSpeed, float travelDistance, float maxScale)
        {
            _maxScale = maxScale;
            _travelDistance = travelDistance;
            _speed = ballConfigSpeed;
            _direction = direction;
            _initialScale = transform.localScale.x;
            _startPosition = transform.position;
        }

        public void SetSprite(Sprite sprite) =>
                _spriteRenderer.sprite = sprite;

        public async UniTask MoveAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && this)
            {
                transform.Translate(_direction * (_speed * _timeService.DeltaTime), Space.World);

                var distanceTraveled = Vector3.Distance(_startPosition, transform.position);
                var progress = Mathf.Clamp01(distanceTraveled / _travelDistance);

                var currentScale = Mathf.Lerp(_initialScale, _maxScale, progress);
                transform.localScale = new(currentScale, currentScale, 1f);
                _ballScaleProvider.UpdateScale(currentScale);
                
                if(IsOutOfBounds() && _destructed == false)
                {
                    BallDestroyedSubject?.OnNext(Unit.Default);
                    Destroy(gameObject);
                    break;
                }

                await UniTask.Yield(cancellationToken);
            }
        }

        private bool IsOutOfBounds()
        {
            var viewportPos = _cameraProvider.MainCamera.WorldToViewportPoint(transform.position);
            var viewportPosX = viewportPos.x;
            var viewportPosY = viewportPos.y;

            return viewportPosX < -MinDistance || viewportPosX > MaxDistance || viewportPosY < -MinDistance || viewportPosY > MaxDistance;
        }

        public async UniTask PlayDestructed()
        {
            _destructed = true;
            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
            transform.DORotate(new(0, 0, 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear);

            await UniTask.Delay(500);

            Destroy(gameObject);
        }
    }
}
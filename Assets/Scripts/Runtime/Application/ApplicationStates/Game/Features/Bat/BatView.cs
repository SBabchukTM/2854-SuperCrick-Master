using Cysharp.Threading.Tasks;
using R3;
using Runtime.Application.ApplicationStates.Game.Features.Enemy;
using UnityEngine;

namespace Runtime.Application.ApplicationStates.Game.Features.Bat
{
    public class BatView : MonoBehaviour
    {
        public readonly Subject<Unit> HitBallSubject = new();
        public readonly Subject<Unit> HitSoundSubject = new();

        [SerializeField] private LayerMask _ballLayerMask;
        [SerializeField] private Transform _pivot;
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private float rotationSpeed = 100f;
        [SerializeField] private float maxRotationAngle = 45f;

        [HideInInspector] public bool IsReadyToProtect;

        private Vector2 _previousPosition;
        private bool _isRotating;
        private float _currentRotation;
        private SpriteRenderer _spriteRenderer;
        private PolygonCollider2D _collider;

        private Vector2 _castDirection;
        private Vector2 _boxSize;
        private float _castDistance;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<PolygonCollider2D>();
            _currentRotation = _pivot.localEulerAngles.z;
        }

        private void Update()
        {
            if(IsReadyToProtect)
                CheckCollisionWithBall().Forget();
        }

        private void OnMouseDown()
        {
            _isRotating = true;
            _previousPosition = Input.mousePosition;
        }

        private void OnMouseUp() =>
            _isRotating = false;

        private void OnMouseDrag()
        {
            if (!_isRotating || !_pivot)
                return;

            var currentPosition = (Vector2)Input.mousePosition;
            var direction = currentPosition - _previousPosition;

            var angleDelta = direction.y * Time.deltaTime * rotationSpeed * 0.1f;
            _currentRotation += angleDelta;
            _currentRotation = Mathf.Clamp(_currentRotation, -maxRotationAngle, maxRotationAngle);

            _pivot.localRotation = Quaternion.Euler(0, 0, _currentRotation);

            _previousPosition = currentPosition;
        }

        public void SetSprite(Sprite sprite) =>
            _spriteRenderer.sprite = sprite;

        private async UniTask CheckCollisionWithBall()
        {
            _castDirection = _attackPoint.up;

            _boxSize = _collider.bounds.size / 3;
            _castDistance = _boxSize.magnitude;

            var hit = Physics2D.BoxCast(_attackPoint.position, _boxSize, _attackPoint.eulerAngles.z, _castDirection, _castDistance, _ballLayerMask);

            if (hit.collider)
            {
                hit.collider.enabled = false;

                HitSoundSubject?.OnNext(Unit.Default);
                
                if(hit.transform.gameObject.TryGetComponent(out Ball ball))
                    await ball.PlayDestructed();

                HitBallSubject?.OnNext(Unit.Default);
            }
        }

        private void OnDrawGizmos()
        {
            if(_attackPoint == null || _collider == null)
                return;

            Gizmos.color = Color.red;
            Vector3 boxCenter = _attackPoint.position + (Vector3)_castDirection * _castDistance * 0.5f;
            Gizmos.matrix = Matrix4x4.TRS(boxCenter, _attackPoint.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, _boxSize);
        }
    }
}
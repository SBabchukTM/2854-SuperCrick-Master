using Application.Services.CameraProvider;
using UnityEngine;
using Zenject;

namespace Application.Services.Resize
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteFullScreenResizer : MonoBehaviour
    {
        private ICameraProvider _cameraProvider;
        private SpriteRenderer _spriteRenderer;

        [Inject]
        public void Construct(ICameraProvider cameraProvider) =>
                _cameraProvider = cameraProvider;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if(!_cameraProvider?.MainCamera)
            {
                Debug.LogError("No main camera on provider.");

                return;
            }

            ResizeToFitCamera();
        }

        private void ResizeToFitCamera()
        {
            var worldScreenWidth = _cameraProvider.WorldScreenWidth;
            var worldScreenHeight = _cameraProvider.WorldScreenHeight;

            Vector2 spriteSize = _spriteRenderer.sprite.bounds.size;
            var scale = transform.localScale;

            scale.x = worldScreenWidth / spriteSize.x;
            scale.y = worldScreenHeight / spriteSize.y;

            transform.localScale = scale;
            transform.position = new(0, 0, transform.position.z);
        }
    }
}
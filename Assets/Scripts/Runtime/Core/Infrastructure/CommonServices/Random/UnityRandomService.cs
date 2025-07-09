using Application.Services.CameraProvider;
using UnityEngine;

namespace Code.Gameplay.Common.Random
{
    public class UnityRandomService : IRandomService
    {
        private readonly ICameraProvider _cameraProvider;

        public UnityRandomService(ICameraProvider cameraProvider) =>
                _cameraProvider = cameraProvider;

        public float Range(float inclusiveMin, float inclusiveMax) => UnityEngine.Random.Range(inclusiveMin, inclusiveMax);

        public int Range(int inclusiveMin, int exclusiveMax) => UnityEngine.Random.Range(inclusiveMin, exclusiveMax);

        public Vector2 GetRandomDirection(Vector2 position)
        {
            var goLeft = Range(0f, 1f) > 0.5f;

            const float viewportY = 0.5f;
            const float maxVerticalOffset = 0.2f;

            var viewportEdge = new Vector3(goLeft ? 0 : 1, viewportY, _cameraProvider.MainCamera.nearClipPlane);
            Vector2 targetEdge = _cameraProvider.MainCamera.ViewportToWorldPoint(viewportEdge);

            var direction = (targetEdge - position).normalized;
            direction.y = Mathf.Clamp(direction.y, -maxVerticalOffset, maxVerticalOffset);

            return direction.normalized;
        }        
    }
}
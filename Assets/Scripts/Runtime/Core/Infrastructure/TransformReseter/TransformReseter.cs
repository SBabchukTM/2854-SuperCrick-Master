using UnityEngine;

namespace Application.Game
{
    public class TransformReseter : ITransformReseter
    {
        private readonly Vector2 _spawnedPosition;
        private readonly Quaternion _spawnedRotation;

        public TransformReseter(Transform transform)
        {
            _spawnedRotation = transform.localRotation;
            _spawnedPosition = transform.localPosition;
        }

        public void ResetPositions(Transform currentPosition)
        {
            currentPosition.localRotation = _spawnedRotation;
            currentPosition.localPosition = _spawnedPosition;
        }
    }
}
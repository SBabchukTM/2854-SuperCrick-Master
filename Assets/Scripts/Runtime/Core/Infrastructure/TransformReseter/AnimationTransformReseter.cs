using DG.Tweening;
using UnityEngine;

namespace Application.Game
{
    public class AnimationTransformReseter : ITransformReseter
    {
        private readonly Vector2 _spawnedPosition;
        private readonly Quaternion _spawnedRotation;
        private readonly float _time;

        public AnimationTransformReseter(Transform transform, float time)
        {
            _time = time;
            _spawnedRotation = transform.localRotation;
            _spawnedPosition = transform.localPosition;
        }

        public void ResetPositions(Transform currentPosition)
        {
            currentPosition.DOLocalMove(_spawnedPosition, _time).SetEase(Ease.InCirc).SetUpdate(true);
            currentPosition.DOLocalRotateQuaternion(_spawnedRotation, _time).SetEase(Ease.InCirc).SetUpdate(true);
        }
    }
}
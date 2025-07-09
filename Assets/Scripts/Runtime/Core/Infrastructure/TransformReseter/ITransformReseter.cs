using UnityEngine;

namespace Application.Game
{
    public interface ITransformReseter
    {
        void ResetPositions(Transform currentPosition);
    }
}
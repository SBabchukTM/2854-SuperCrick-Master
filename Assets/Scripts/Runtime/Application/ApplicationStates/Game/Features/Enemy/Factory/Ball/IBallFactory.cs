using UnityEngine;

namespace Runtime.Application.ApplicationStates.Game.Features.Enemy.Factory
{
    public interface IBallFactory
    {
        Ball Create(Vector2 at, GameObject view, Sprite sprite);
    }
}
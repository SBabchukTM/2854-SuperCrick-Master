using UnityEngine;

namespace Runtime.Application.ApplicationStates.Game.Features.Enemy.Factory
{
    public interface IEnemyFactory
    {
        Enemy Create(Vector2 at, GameObject view);
    }
}
using Core.Factory;
using UnityEngine;

namespace Runtime.Application.ApplicationStates.Game.Features.Enemy.Factory
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly GameObjectFactory _gameObjectFactory;

        public EnemyFactory(GameObjectFactory gameObjectFactory) =>
                _gameObjectFactory = gameObjectFactory;

        public Enemy Create(Vector2 at, GameObject view)
        {
            var enemyPrefab = _gameObjectFactory.Create(view, at, Quaternion.identity, null);
            var enemyComponent = enemyPrefab.GetComponent<Enemy>();

            return enemyComponent;
        }
    }
}
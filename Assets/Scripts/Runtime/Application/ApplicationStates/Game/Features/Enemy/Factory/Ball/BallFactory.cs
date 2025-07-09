using Core.Factory;
using UnityEngine;

namespace Runtime.Application.ApplicationStates.Game.Features.Enemy.Factory
{
    public class BallFactory : IBallFactory
    {
        private readonly GameObjectFactory _gameObjectFactory;

        public BallFactory(GameObjectFactory gameObjectFactory) =>
                _gameObjectFactory = gameObjectFactory;

        public Ball Create(Vector2 at, GameObject view, Sprite sprite)
        {
            var ballPrefab = _gameObjectFactory.Create(view, at, Quaternion.identity, null);
            var ballComponent = ballPrefab.GetComponent<Ball>();
            ballComponent.SetSprite(sprite);
            ballPrefab.AddComponent<PolygonCollider2D>();
            
            return ballComponent;
        }
    }
}
using Core.Factory;
using Runtime.Application.ApplicationStates.Game.Features.Bat.Runtime.Application.ApplicationStates.Game.Features.Bat;
using UnityEngine;

namespace Runtime.Application.ApplicationStates.Game.Features.Bat.Factory
{
    public class BatLauncherFactory : IBatLauncherFactory
    {
        private readonly GameObjectFactory _gameObjectFactory;

        public BatLauncherFactory(GameObjectFactory gameObjectFactory) =>
                _gameObjectFactory = gameObjectFactory;

        public BatController Create(Vector2 at, GameObject view, Sprite batSprite)
        {
            var batLauncherPrefab = _gameObjectFactory.Create(view, at, Quaternion.identity, null);
            var batController = batLauncherPrefab.GetComponent<BatController>();
            batController.SetBatSprite(batSprite);

            return batController;
        }
    }
}
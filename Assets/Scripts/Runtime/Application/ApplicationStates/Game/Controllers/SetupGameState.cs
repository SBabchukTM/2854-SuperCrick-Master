using Application.Services.CameraProvider;
using Application.UI;
using Core.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Application.Game
{
    public class SetupGameState : ISetupGameState
    {
        private readonly ICameraProvider _cameraProvider;
        private readonly GameObjectFactory _gameObjectFactory;
        private readonly ILevelPointProvider _levelPointProvider;

        public SetupGameState(ICameraProvider cameraProvider, GameObjectFactory gameObjectFactory, ILevelPointProvider levelPointProvider)
        {
            _cameraProvider = cameraProvider;
            _gameObjectFactory = gameObjectFactory;
            _levelPointProvider = levelPointProvider;
        }

        public async UniTask Setup()
        {
            _cameraProvider.SetMainCamera(Camera.main);
            var background = await _gameObjectFactory.Create<Background>(ConstGame.Background);
            background.SetLevelPointerProvider(_levelPointProvider);            
        }
    }
}
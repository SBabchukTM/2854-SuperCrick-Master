using UnityEngine;
using Zenject;

namespace Runtime.Application.ApplicationStates.Game.Player
{
    public class ScreenSizeCalculator : IScreenSizeChecker, IInitializable
    {
        private Camera _mainCamera;
        private Vector2 _screenSize;
        private Vector2 _screenCenter;
        private Vector2 _screenBounds;

        public void Initialize()
        {
            _mainCamera = Camera.main;
            CalculateScreenBounds();
        }

        public Vector2 GetScreenSize()
        {
            return _screenSize;
        }

        public Vector2 GetScreenCenter()
        {
            return _screenCenter;
        }

        public Vector2 GetScreenBounds()
        {
            return _screenBounds;
        }

        private void CalculateScreenBounds()
        {
            if (!_mainCamera)
                return;

            _screenSize = new Vector2(Screen.width, Screen.height);
            _screenCenter = _screenSize / 2f;
            _screenBounds = _mainCamera.ScreenToWorldPoint(new Vector3(_screenSize.x, _screenSize.y, 0));
        }
    }
}
using UnityEngine;

namespace Runtime.Application.ApplicationStates.Game.Player
{
    public interface IScreenSizeChecker
    {
        Vector2 GetScreenSize();
        Vector2 GetScreenCenter();
        Vector2 GetScreenBounds();
    }
}
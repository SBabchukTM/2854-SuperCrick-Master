using Application.UI;
using UnityEngine;

namespace Application.Services.Pause
{
    public static class PauseTaker
    {
        public static void TakePause(GameStateTypeId gameState) =>
                Time.timeScale = gameState == GameStateTypeId.RunningState ? 1 : 0;
    }
}
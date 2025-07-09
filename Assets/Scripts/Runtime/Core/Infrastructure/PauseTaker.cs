using UnityEngine;

namespace Application.Game
{
    public static class PauseTaker
    {
        public static void TakePause(int timeValue)
        {
            Time.timeScale = timeValue;
        }
    }
}
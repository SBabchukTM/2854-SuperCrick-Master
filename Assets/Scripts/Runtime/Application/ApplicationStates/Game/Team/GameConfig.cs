using Core;
using UnityEngine;

namespace Application.Game
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Config/Game/GameConfig")]
    public sealed class GameConfig : BaseSettings
    {
        public int ScoreToWin;
    }
}
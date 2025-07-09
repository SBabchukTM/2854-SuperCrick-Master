using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Application.Game
{
    [CreateAssetMenu(fileName = "GameResultSetup", menuName = "Config/GameResult/GameResultSetup")]
    public sealed class GameResultSetup : BaseSettings
    {
        public List<GameResultConfig> GameResultConfigs;
    }
}
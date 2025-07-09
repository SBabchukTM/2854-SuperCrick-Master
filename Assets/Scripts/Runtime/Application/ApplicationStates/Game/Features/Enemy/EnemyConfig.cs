using Core;
using UnityEngine;

namespace Runtime.Application.ApplicationStates.Game.Features.Bat
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Config/Game/EnemyConfig")]
    public sealed class EnemyConfig : BaseSettings
    {
        public GameObject View;
        public float TimeToCreateBall;
    }
}
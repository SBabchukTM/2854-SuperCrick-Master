using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Runtime.Application.ApplicationStates.Game.Features.Bat
{
    [CreateAssetMenu(fileName = "BallConfig", menuName = "Config/Game/BallConfig")]
    public sealed class BallConfig : BaseSettings
    {
        public List<Sprite> Sprites;
        public GameObject View;
        public float Speed;
        public float MaxScale;
    }
}
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Runtime.Application.ApplicationStates.Game.Features.Bat
{
    [CreateAssetMenu(fileName = "BatConfig", menuName = "Config/Game/BatConfig")]
    public sealed class BatConfig : BaseSettings
    {
        public List<Sprite> Sprites;
        public GameObject View;
    }
}
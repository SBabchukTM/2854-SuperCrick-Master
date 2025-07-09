using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Application.Game
{
    [CreateAssetMenu(fileName = "TeamConfig", menuName = "Config/Game/TeamConfig")]
    public sealed class TeamConfig : BaseSettings
    {
        public List<Sprite> Sprites;
        public GameObject View;
    }
}
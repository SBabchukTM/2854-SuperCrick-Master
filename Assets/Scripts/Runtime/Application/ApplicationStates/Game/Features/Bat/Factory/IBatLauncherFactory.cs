using Runtime.Application.ApplicationStates.Game.Features.Bat.Runtime.Application.ApplicationStates.Game.Features.Bat;
using UnityEngine;

namespace Runtime.Application.ApplicationStates.Game.Features.Bat.Factory
{
    public interface IBatLauncherFactory
    {
        BatController Create(Vector2 at, GameObject view, Sprite batSprite);
    }
}
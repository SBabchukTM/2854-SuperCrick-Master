using R3;

namespace Runtime.Application.ApplicationStates.Game.Features.Bat
{
    public class BallScaleProvider : IBallScaleProvider
    {
        public ReactiveProperty<float> Scale { get; } = new();

        public void UpdateScale(float scale) =>
                Scale.OnNext(scale);
    }
}
using R3;

namespace Runtime.Application.ApplicationStates.Game.Features.Bat
{
    public interface IBallScaleProvider
    {
        ReactiveProperty<float> Scale { get; }

        void UpdateScale(float scale);
    }
}
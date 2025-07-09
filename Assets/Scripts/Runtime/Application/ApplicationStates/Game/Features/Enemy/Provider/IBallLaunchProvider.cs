using R3;

namespace Application.Game
{
    public interface IBallLaunchProvider
    {
        Subject<Unit> ReadyToLaunchSubject { get; }

        void Launch();
    }
}
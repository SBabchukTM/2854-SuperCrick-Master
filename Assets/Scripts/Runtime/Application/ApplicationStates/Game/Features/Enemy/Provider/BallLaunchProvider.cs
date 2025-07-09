using R3;

namespace Application.Game
{
    public class BallLaunchProvider : IBallLaunchProvider
    {
        public Subject<Unit> ReadyToLaunchSubject { get; private set; } = new();
        
        public void Launch() =>
                ReadyToLaunchSubject.OnNext(Unit.Default);
    }
}
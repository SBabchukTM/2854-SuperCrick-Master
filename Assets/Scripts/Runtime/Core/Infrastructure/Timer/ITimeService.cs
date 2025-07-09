using System;

namespace ImprovedTimers
{
    public interface ITimeService
    {
        float DeltaTime { get; }
        DateTime UtcNow { get; }

        void StopTime();

        void StartTime();
    }
}
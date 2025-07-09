using System;

namespace ImprovedTimers
{
    public class TimeService : ITimeService
    {
        private bool _paused;

        public float DeltaTime => !_paused ? UnityEngine.Time.deltaTime : 0;

        public DateTime UtcNow => DateTime.UtcNow;
    
        public void StopTime() => _paused = true;
        public void StartTime() => _paused = false;
    }
}
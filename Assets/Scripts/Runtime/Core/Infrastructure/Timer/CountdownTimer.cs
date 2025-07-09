using UnityEngine;

namespace ImprovedTimers
{
    public class CountdownTimer : Timer
    {
        public CountdownTimer(float value) : base(value)
        {
        }

        public override void Tick()
        {
            if (IsRunning)
                CurrentTime += Time.deltaTime;
            
            if (IsRunning && CurrentTime >= MaxTime)
                Stop();
        }

        public override bool IsFinished => CurrentTime >= MaxTime;
    }
}
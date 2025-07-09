using System;
using Application.Game;
using UnityEngine;

namespace ImprovedTimers
{
    public abstract class Timer : IDisposable
    {
        private readonly Action _onTimerStart = delegate { };

        protected float MaxTime;
        
        protected float CurrentTime { get; set; }
        public bool IsRunning { get; private set; }
        
        public float Progress => Mathf.Clamp(CurrentTime, 0, MaxTime);

        public Action OnTimerStop = delegate { };

        protected Timer(float value)
        {
            MaxTime = value;
        }

        public void Start()
        {
            CurrentTime = 0;
            if (!IsRunning)
            {
                IsRunning = true;
                TimerManager.RegisterTimer(this);
                _onTimerStart.Invoke();
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                TimerManager.DeregisterTimer(this);
                OnTimerStop.Invoke();
            }
        }

        public abstract void Tick();
        public abstract bool IsFinished { get; }

        public void Resume()
        {
            IsRunning = true;
        }

        public void Pause()
        {
            IsRunning = false;
        }

        public virtual void Reset()
        {
            CurrentTime = 0;
        }

        public virtual void Reset(float newTime)
        {
            MaxTime = newTime;
            Reset();
        }

        private bool _disposed;

        ~Timer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                TimerManager.DeregisterTimer(this);

            _disposed = true;
        }
    }
}
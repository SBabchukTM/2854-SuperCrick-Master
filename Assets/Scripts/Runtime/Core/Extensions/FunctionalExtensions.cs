using System;
using R3;

namespace Runtime.Core.Extensions
{
    public static class FunctionalExtensions
    {
        public static T With<T>(this T self, Action<T> set)
        {
            set?.Invoke(self);
            return self;
        }
        public static T With<T>(this T self, Action set)
        {
            set?.Invoke();

            return self;
        }

        public static T With<T>(this T self, Action<T> apply, bool when)
        {
            if (when)
                apply?.Invoke(self);
            return self;
        }

        public static T With<T>(this T self, Subject<T> set)
        {
            set?.OnNext(self);
            return self;
        }

        public static T With<T>(this T self, Subject<T> apply, bool when)
        {
            if (when)
                apply?.OnNext(self);
            return self;
        }

        public static T With<T>(this T self, Subject<Unit> set)
        {
            set?.OnNext(Unit.Default); 
            return self;
        }

        public static T With<T>(this T self, Subject<Unit> apply, bool when)
        {
            if (when)
                apply?.OnNext(Unit.Default); 
            return self;
        }
    }
}
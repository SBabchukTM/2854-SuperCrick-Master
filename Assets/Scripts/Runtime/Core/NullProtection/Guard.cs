using System;
using System.Collections.Generic;

namespace Utils
{
    public static class Guard
    {
        public static T NotNull<T>(T value, string paramName) where T : class => value ?? throw new ArgumentNullException(paramName);

        public static string NotNullOrWhiteSpace(string value, string paramName) => value switch
        {
                null => throw new ArgumentNullException(paramName),
                { Length: 0 } => throw new ArgumentException($"{paramName} cannot be empty"),
                _ when string.IsNullOrWhiteSpace(value) => throw new ArgumentException($"{paramName} cannot be whitespace"),
                _ => value
        };

        public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> value, string paramName)
        {
            if(value == null || value.Count == 0)
                throw new ArgumentException("Collection cannot be null or empty", paramName);

            return value;
        }

        public static Dictionary<TKey, TValue> NotNullOrEmpty<TKey, TValue>(Dictionary<TKey, TValue> dict, string paramName)
            => dict switch
            {
                    null => throw new ArgumentNullException(paramName),
                    { Count: 0 } => throw new ArgumentException($"{paramName} cannot be empty"),
                    _ => dict
            };

        public static T PositiveValue<T>(T value, string paramName) where T : IComparable<T>
        {
            if(Comparer<T>.Default.Compare(value, default) <= 0)
                throw new ArgumentException("Value must be positive", paramName);

            return value;
        }
    }
}
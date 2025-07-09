using System.Collections.Generic;
using R3;

namespace Application.Game
{
    public class ReactiveDictionary<TKey, TValue>
    {
        public readonly Dictionary<TKey, TValue> Dictionary = new();
        
        public Subject<(TKey Key, TValue Value)> OnAdd { get; } = new();
        public Subject<TKey> OnRemove { get; } = new();
        public Subject<(TKey Key, TValue OldValue, TValue NewValue)> OnUpdate { get; } = new();

        public void Add(TKey key, TValue value)
        {
            Dictionary[key] = value;
            OnAdd.OnNext((key, value));
        }

        public void Remove(TKey key)
        {
            if (Dictionary.Remove(key))
                OnRemove.OnNext(key);
        }

        public void Update(TKey key, TValue newValue)
        {
            if (Dictionary.TryGetValue(key, out var oldValue))
            {
                Dictionary[key] = newValue;
                OnUpdate.OnNext((key, oldValue, newValue));
            }
        }

        public TValue this[TKey key]
        {
            get => Dictionary[key];
            set => Update(key, value);
        }

        public bool TryGetValue(TKey key, out TValue value) => 
                Dictionary.TryGetValue(key, out value);
    }
}
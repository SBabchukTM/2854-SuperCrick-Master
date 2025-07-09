using System.Collections.Generic;
using R3;
using Runtime.Core.Extensions;
using UnityEngine.UI;

namespace Application.UI
{
    public class DictionaryButtonSubscriber : IDictionaryButtonSubscriber
    {
        private readonly Dictionary<Button, Subject<Unit>> _invokers;
        
        public DictionaryButtonSubscriber(Dictionary<Button, Subject<Unit>> invokers) =>
                _invokers = invokers;

        public void SubscribeAll()
        {
            foreach (var pair in _invokers)
                pair.Key.SubscribeButtonClick(pair.Value);
        }
    }
}
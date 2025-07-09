using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core.Factory
{
    public class GameObjectFactory
    {
        private readonly Dictionary<string, GameObject> _cachedAddressables;
        private readonly IInstantiator _instantiator;
        private readonly IAssetProvider _assetProvider;

        public GameObjectFactory(DiContainer instantiator, IAssetProvider assetProvider)
        {
            _cachedAddressables = new();
            _instantiator = instantiator;
            _assetProvider = assetProvider;
        }

        public GameObject Create(GameObject prototype) =>
                _instantiator.InstantiatePrefab(prototype);

        public GameObject Create(GameObject prototype, Transform parent) =>
                _instantiator.InstantiatePrefab(prototype, parent);

        public GameObject Create(GameObject prototype, Vector3 position, Quaternion rotation, Transform parent) =>
                _instantiator.InstantiatePrefab(prototype, position, rotation, parent);

        public TComponent Create<TComponent>(GameObject prototype) where TComponent : Component =>
                _instantiator.InstantiatePrefabForComponent<TComponent>(prototype);

        public TComponent Create<TComponent>(GameObject prototype, Transform parent) where TComponent : Component =>
                _instantiator.InstantiatePrefabForComponent<TComponent>(prototype, parent);

        public TComponent Create<TComponent>(GameObject prototype, Vector3 position, Quaternion rotation, Transform parent)
                where TComponent : Component =>
                _instantiator.InstantiatePrefabForComponent<TComponent>(prototype, position, rotation, parent);

        public TComponent Create<TComponent>(TComponent prototype) where TComponent : Component =>
                _instantiator.InstantiatePrefabForComponent<TComponent>(prototype);

        public TComponent Create<TComponent>(TComponent prototype, Transform parent) where TComponent : Component =>
                _instantiator.InstantiatePrefabForComponent<TComponent>(prototype, parent);

        public TComponent Create<TComponent>(TComponent prototype, Vector3 position, Quaternion rotation, Transform parent)
                where TComponent : Component =>
                _instantiator.InstantiatePrefabForComponent<TComponent>(prototype, position, rotation, parent);

        public async UniTask<GameObject> Create(string addressableName) =>
                _instantiator.InstantiatePrefab(await GetPrototype(addressableName));

        public async UniTask<GameObject> Create(string addressableName, Transform parent) =>
                _instantiator.InstantiatePrefab(await GetPrototype(addressableName), parent);

        public async UniTask<GameObject> Create(string addressableName, Vector3 position, Quaternion rotation, Transform parent) =>
                _instantiator.InstantiatePrefab(await GetPrototype(addressableName), position, rotation, parent);

        public async UniTask<TComponent> Create<TComponent>(string addressableName) where TComponent : Component
        {
            var prototype = await GetPrototype(addressableName);
            var component = _instantiator.InstantiatePrefabForComponent<TComponent>(prototype);
            component.gameObject.name = prototype.name;

            return component;
        }

        public async UniTask<TComponent> Create<TComponent>(string addressableName, Transform parent) where TComponent : Component
        {
            var prototype = await GetPrototype(addressableName);
            var component = _instantiator.InstantiatePrefabForComponent<TComponent>(prototype, parent);
            component.gameObject.name = prototype.name;

            return component;
        }

        public async UniTask<TComponent> Create<TComponent>(string addressableName, Vector3 position, Quaternion rotation, Transform parent)
                where TComponent : Component
        {
            var prototype = await GetPrototype(addressableName);
            var component = _instantiator.InstantiatePrefabForComponent<TComponent>(prototype, position, rotation, parent);
            component.gameObject.name = prototype.name;

            return component;
        }

        private async UniTask<GameObject> GetPrototype(string addressableName)
        {
            if(_cachedAddressables.TryGetValue(addressableName, out var prototype))
                return prototype;

            prototype = await _assetProvider.Load<GameObject>(addressableName);

            _cachedAddressables[addressableName] = prototype;

            return prototype;
        }
    }
}
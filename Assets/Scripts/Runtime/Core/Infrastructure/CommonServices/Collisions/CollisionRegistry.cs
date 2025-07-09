using System.Collections.Generic;
using Code.Gameplay.Common.Collisions;
using UnityEngine;

public class CollisionRegistry : ICollisionRegistry
{
    private readonly Dictionary<int, GameObject> _entityByInstanceId = new();

    public void Register(int instanceId, GameObject entity) => _entityByInstanceId[instanceId] = entity;

    public void Unregister(int instanceId)
    {
        if(_entityByInstanceId.ContainsKey(instanceId))
            _entityByInstanceId.Remove(instanceId);
    }

    public TCollission Get<TCollission>(int instanceId) where TCollission : class
    {
        return _entityByInstanceId.TryGetValue(instanceId, out var gameObject) ? gameObject.GetComponent<TCollission>() : null;
    }
}
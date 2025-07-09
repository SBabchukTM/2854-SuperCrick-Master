using UnityEngine;

namespace Code.Gameplay.Common.Collisions
{
    public interface ICollisionRegistry
    {
        void Register(int instanceId, GameObject entity);
        
        void Unregister(int instanceId);
        
        TCollission Get<TCollission>(int instanceId) where TCollission : class;
    }
}
using System.Collections.Generic;
using System.Linq;

namespace Application.Game
{
    public class DestroyableService : IDestroyableService
    {
        private readonly HashSet<IDestroyable> _destroyables = new();
        
        public void DestroyAll()
        {
            foreach (var destroyable in _destroyables.ToList())
            {
                destroyable?.Destroy();
            }

            _destroyables.Clear();
        }

        public void RegisterDestroyable(IDestroyable destroyable) => _destroyables.Add(destroyable);

        public void UnregisterDestroyable(IDestroyable destroyable) => _destroyables.Remove(destroyable);
    }
}
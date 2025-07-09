using UnityEngine;
using Zenject;

namespace Application.Game
{
    public abstract class Destroyable : MonoBehaviour, IDestroyable
    {
        private IDestroyableService _destroyableService;

        [Inject]
        public void Construct(IDestroyableService destroyableService)
        {
            _destroyableService = destroyableService;
        }

        private void OnEnable()
        {
            _destroyableService.RegisterDestroyable(this);
        }

        protected virtual void OnDestroy()
        {
            _destroyableService.UnregisterDestroyable(this);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
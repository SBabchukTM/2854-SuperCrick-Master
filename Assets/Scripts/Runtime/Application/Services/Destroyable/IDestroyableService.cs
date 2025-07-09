namespace Application.Game
{
    public interface IDestroyableService
    {
        void DestroyAll();

        void RegisterDestroyable(IDestroyable destroyable);

        void UnregisterDestroyable(IDestroyable destroyable);
    }
}
using System.Collections.Generic;

namespace Application.Game
{
    public interface IFinishedComponentsService
    {
        HashSet<IFinished> GetAllFinished();

        void RegisterIFinished(IFinished finished);

        void UnregisterIFinished(IFinished finished);
    }
}
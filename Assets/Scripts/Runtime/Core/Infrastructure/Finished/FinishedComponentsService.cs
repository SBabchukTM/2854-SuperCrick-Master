using System.Collections.Generic;

namespace Application.Game
{
    public class FinishedComponentsService : IFinishedComponentsService
    {
        private readonly HashSet<IFinished> _finishedComponents = new();

        public HashSet<IFinished> GetAllFinished() => _finishedComponents;

        public void RegisterIFinished(IFinished finished) => _finishedComponents.Add(finished);

        public void UnregisterIFinished(IFinished finished) => _finishedComponents.Remove(finished);
    }
}
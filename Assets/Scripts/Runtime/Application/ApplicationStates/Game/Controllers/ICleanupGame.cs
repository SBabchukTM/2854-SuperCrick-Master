using System.Threading;
using Cysharp.Threading.Tasks;

namespace Application.Game
{
    public interface ICleanupGame
    {
        UniTask Cleanup(CancellationToken cancellationToken);
    }
}
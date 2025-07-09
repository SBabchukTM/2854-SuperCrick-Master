using Cysharp.Threading.Tasks;

namespace Application.Game
{
    public interface ISetupGameState
    {
        UniTask Setup();
    }
}
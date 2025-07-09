using Cysharp.Threading.Tasks;

namespace Application.Game
{
    public interface ILosableAsync
    {
        UniTask LoseAsync();
    }
}
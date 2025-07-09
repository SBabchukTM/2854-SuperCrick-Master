using Cysharp.Threading.Tasks;

namespace Runtime.Application.UI.LevelSelectionButtonHandlers
{
    public interface ILevelSelectionButtonCreator
    {
        UniTask CreateSkinSelectionButtons(int levelCount);
    }
}
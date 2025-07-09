using System.Threading;
using Application.UI;
using Cysharp.Threading.Tasks;

namespace Application.Game
{
    public class CleanupGame : ICleanupGame
    {
        private readonly IDestroyableService _destroyableService;
        private readonly IUiService _uiService;

        public CleanupGame(IDestroyableService destroyableService, IUiService uiService)
        {
            _destroyableService = destroyableService;
            _uiService = uiService;
        }
        public async UniTask Cleanup(CancellationToken cancellationToken)
        {
            _destroyableService.DestroyAll();
            await _uiService.HideScreen(ConstScreens.GameScreen, cancellationToken);
            await UniTask.NextFrame();
        }
    }
}
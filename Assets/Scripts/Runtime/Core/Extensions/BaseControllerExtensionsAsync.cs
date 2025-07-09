using System.Threading;
using Core;
using Cysharp.Threading.Tasks;

namespace Runtime.Core.Extensions
{
    public static class BaseControllerExtensionsAsync
    {
        public static async UniTask StopIfRunningAsync(this BaseController baseController)
        {
            if(baseController.CurrentControllerState == ControllerState.Run) 
                await baseController.Stop();
        }
        
        public static async UniTask RunIfNotRunningAsync(this BaseController baseController, CancellationToken cancellationToken)
        {
            if(baseController.CurrentControllerState != ControllerState.Run) 
                await baseController.Run(cancellationToken);
        }
        
        public static async UniTask StopAndRunAsync(this BaseController baseController, CancellationToken cancellationToken)
        {
            await StopIfRunningAsync(baseController);
            await RunIfNotRunningAsync(baseController, cancellationToken);
        }
    }
}
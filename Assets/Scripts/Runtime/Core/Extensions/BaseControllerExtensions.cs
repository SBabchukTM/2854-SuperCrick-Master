using System.Threading;
using Core;
using Cysharp.Threading.Tasks;

namespace Runtime.Core.Extensions
{
    public static class BaseControllerExtensions
    {
        public static void StopIfRunning(this BaseController baseController)
        {
            if(baseController.CurrentControllerState == ControllerState.Run) 
                baseController.Stop().Forget();
        }
        
        public static void RunIfNotRunning(this BaseController baseController, CancellationToken cancellationToken)
        {
            if(baseController.CurrentControllerState != ControllerState.Run) 
                baseController.Run(cancellationToken).Forget();
        }
        
        public static void StopAndRun(this BaseController baseController, CancellationToken cancellationToken)
        {
            StopIfRunning(baseController);
            RunIfNotRunning(baseController, cancellationToken);
        }
    }
}
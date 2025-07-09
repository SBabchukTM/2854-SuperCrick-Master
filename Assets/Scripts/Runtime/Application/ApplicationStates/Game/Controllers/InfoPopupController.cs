using System.Threading;
using Application.UI;
using Core;
using Cysharp.Threading.Tasks;
using R3;
using Runtime.Core.Extensions;

namespace Application.Game
{
    public class InfoPopupController : BaseController
    {
        private readonly IUiService _uiService;

        public InfoPopupController(IUiService uiService) 
        {
            _uiService = uiService;
        }

        public override UniTask Run(CancellationToken cancellationToken)
        {
            var infoPopup = _uiService.GetPopup<ClosablePopup>(ConstPopups.InfoPopup);

            infoPopup.Show(new InfoPopupData(), cancellationToken);

            infoPopup.CloseButtonPressedSubject
                    .Subscribe(_ => this.StopIfRunning());
            
            return base.Run(cancellationToken);
        }
    }
}
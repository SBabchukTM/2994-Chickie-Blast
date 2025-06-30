using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.Controllers;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData;
using Runtime.Game.UI.Popup;
using Runtime.Game.UI.Popup.Data;

namespace Runtime.Game.GameStates.Game.Controllers
{
    public sealed class StartSettingsController : BaseController
    {
        private readonly IUiService _uiService;
        private readonly UserDataService _userDataService;
        private readonly IAudioService _audioService;

        public StartSettingsController(IUiService uiService, UserDataService userDataService, IAudioService audioService)
        {
            _uiService = uiService;
            _userDataService = userDataService;
            _audioService = audioService;
        }

        public override UniTask Run(CancellationToken cancellationToken)
        {
            base.Run(cancellationToken);
            
            return UniTask.CompletedTask;
        }
    }
}
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class MenuStateController : StateController
    {
        private readonly IUiService _uiService;

        private MenuScreen _screen;

        public MenuStateController(ILogger logger, IUiService uiService) : base(logger)
        {
            _uiService = uiService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.MenuScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<MenuScreen>(ConstScreens.MenuScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<MainScreenStateController>();
            _screen.OnLeaderboardPressed += async () => await GoTo<LeaderboardStateController>();
            _screen.OnProfilePressed += async () => await GoTo<ProfileStateController>();
            _screen.OnSettingsPressed += async () => await GoTo<SettingsStateController>();
            _screen.OnPrivacyPolicyPressed += async () => await GoTo<PrivacyPolicyStateController>();
            _screen.OnTermsOfUsePressed += async () => await GoTo<TermsOfUseStateController>();
        }
    }
}
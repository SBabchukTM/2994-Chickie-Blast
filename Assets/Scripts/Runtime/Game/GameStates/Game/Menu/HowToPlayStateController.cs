using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.Logger;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class HowToPlayStateController : StateController
    {
        private readonly IUiService _uiService;

        private HowToPlayScreen _screen;

        public HowToPlayStateController(ILogger logger, IUiService uiService) : base(logger)
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
            await _uiService.HideScreen(ConstScreens.HowToPlayScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<HowToPlayScreen>(ConstScreens.HowToPlayScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<MainScreenStateController>();
            _screen.OnMenuPressed += async () => await GoTo<MenuStateController>();
        }
    }
}
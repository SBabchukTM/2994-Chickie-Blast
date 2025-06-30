using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.Logger;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class MainScreenStateController : StateController
    {
        private readonly IUiService _uiService;

        private MainScreen _screen;

        public MainScreenStateController(ILogger logger, IUiService uiService) : base(logger)
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
            await _uiService.HideScreen(ConstScreens.MainScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<MainScreen>(ConstScreens.MainScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBonusPressed += async () => await GoTo<BonusStateController>();
            _screen.OnShopPressed += async () => await GoTo<ShopStateController>();
            _screen.OnMenuPressed += async () => await GoTo<MenuStateController>();
            _screen.OnPlayPressed += async () => await GoTo<SectionSelectStateController>();
            _screen.OnHelpPressed += async () => await GoTo<HowToPlayStateController>();
        }
    }
}
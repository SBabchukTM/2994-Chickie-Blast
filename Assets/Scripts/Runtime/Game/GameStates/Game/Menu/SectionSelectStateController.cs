using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.Logger;
using Runtime.Game.Leaderboard;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class SectionSelectStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly UserProgressService _progressService;
        private readonly GameplayData _gameplayData;

        private SectionSelectScreen _screen;

        public SectionSelectStateController(ILogger logger, IUiService uiService, UserProgressService userProgressService, GameplayData gameplayData) : base(logger)
        {
            _uiService = uiService;
            _progressService = userProgressService;
            _gameplayData = gameplayData;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.SectionSelectScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<SectionSelectScreen>(ConstScreens.SectionSelectScreen);
            
            _screen.Initialize(_progressService.GetSectionOneStars(), 
                _progressService.GetSectionTwoStars(), 
                _progressService.GetSectionThreeStars());
            
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<MainScreenStateController>();
            _screen.OnMenuPressed += async () => await GoTo<MenuStateController>();

            _screen.OnSectionOnePressed += async () =>
            {
                _gameplayData.SectionID = 0;
                await GoTo<LevelSelectStateController>();
            };
            
            _screen.OnSectionTwoPressed += async () =>
            {
                _gameplayData.SectionID = 1;
                await GoTo<LevelSelectStateController>();
            };
            
            _screen.OnSectionThreePressed += async () =>
            {
                _gameplayData.SectionID = 2;
                await GoTo<LevelSelectStateController>();
            };
        }
    }
}
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.Logger;
using Runtime.Game.Leaderboard;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class LevelSelectStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly GameplayData _gameplayData;
        private readonly UserProgressService _progressService;

        private LevelSelectScreen _screen;

        public LevelSelectStateController(ILogger logger, IUiService uiService, 
            GameplayData gameplayData, UserProgressService userProgressService) : base(logger)
        {
            _uiService = uiService;
            _gameplayData = gameplayData;
            _progressService = userProgressService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.LevelSelectScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<LevelSelectScreen>(ConstScreens.LevelSelectScreen);
            _screen.InitializeScreen();
            _screen.InitializeButtons(GetClearData());
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<SectionSelectStateController>();
            _screen.OnMenuPressed += async () => await GoTo<MenuStateController>();
            _screen.OnLevelSelected += async (level) =>
            {
                _gameplayData.LevelId = level;
                await GoTo<GameplayStateController>();
            };
        }

        private List<int> GetClearData()
        {
            switch (_gameplayData.SectionID)
            {
                case 0:
                    return _progressService.GetSectionOneClearData();
                case 1:
                    return _progressService.GetSectionTwoClearData();
                case 2:
                    return _progressService.GetSectionThreeClearData();
            }

            return null;
        }
    }
}
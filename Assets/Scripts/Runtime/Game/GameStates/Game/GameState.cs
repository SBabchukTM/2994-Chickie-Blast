using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Game.GameStates.Game.Controllers;
using Runtime.Game.GameStates.Game.Menu;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game
{
    public class GameState : StateController
    {
        private readonly StateMachine _stateMachine;

        private readonly BonusStateController _bonusController;
        private readonly GameplayStateController _gameplayController;
        private readonly HowToPlayStateController _howToPlayController;
        private readonly LeaderboardStateController _leaderboardController;
        private readonly LevelSelectStateController _levelSelectController;
        private readonly MenuStateController _menuStateController;
        private readonly PrivacyPolicyStateController _privacyPolicyController;
        private readonly ProfileStateController _profileController;
        private readonly SectionSelectStateController _sectionSelectController;
        private readonly SettingsStateController _settingsController;
        private readonly ShopStateController _shopController;
        private readonly TermsOfUseStateController _termsOfUseController;
        private readonly MainScreenStateController _mainScreenController;
        private readonly UserDataStateChangeController _userDataStateChangeController;
        private readonly DailyRewardPopupStateController _dailyRewardPopupController;
        private readonly PausePopupStateController _pausePopupController;
        private readonly WinPopupStateController _winPopupController;

        public GameState(ILogger logger,
            BonusStateController bonusController,
            GameplayStateController gameplayController,
            HowToPlayStateController howToPlayController,
            LeaderboardStateController leaderboardController,
            LevelSelectStateController levelSelectController,
            MenuStateController menuStateController,
            PrivacyPolicyStateController privacyPolicyController,
            ProfileStateController profileController,
            SectionSelectStateController sectionSelectController,
            SettingsStateController settingsController,
            ShopStateController shopController,
            TermsOfUseStateController termsOfUseController,
            MainScreenStateController mainScreenController,
            DailyRewardPopupStateController dailyRewardPopupController,
            PausePopupStateController pausePopupController,
            WinPopupStateController winPopupController,
            StateMachine stateMachine,
            UserDataStateChangeController userDataStateChangeController) : base(logger)
        {
            _stateMachine = stateMachine;
            _bonusController = bonusController;
            _gameplayController = gameplayController;
            _howToPlayController = howToPlayController;
            _leaderboardController = leaderboardController;
            _levelSelectController = levelSelectController;
            _menuStateController = menuStateController;
            _privacyPolicyController = privacyPolicyController;
            _profileController = profileController;
            _sectionSelectController = sectionSelectController;
            _settingsController = settingsController;
            _shopController = shopController;
            _termsOfUseController = termsOfUseController;
            _mainScreenController = mainScreenController;
            _dailyRewardPopupController = dailyRewardPopupController;
            _pausePopupController = pausePopupController;
            _winPopupController = winPopupController;
            _userDataStateChangeController = userDataStateChangeController;
        }

        public override async UniTask Enter(CancellationToken cancellationToken)
        {
            await _userDataStateChangeController.Run(default);

            _stateMachine.Initialize(_bonusController, _gameplayController, 
                _howToPlayController, _leaderboardController, _levelSelectController, _menuStateController,
                _privacyPolicyController, _profileController, _sectionSelectController, 
                _settingsController, _shopController, _termsOfUseController, _mainScreenController,
                _dailyRewardPopupController, _pausePopupController, _winPopupController);
            
            _stateMachine.GoTo<MainScreenStateController>().Forget();
        }
    }
}
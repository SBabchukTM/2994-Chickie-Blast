using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.GameStateMachine;
using Runtime.Game.Bonus;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;
using UnityEngine;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class BonusStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly LoginService _loginService;
        private readonly BonusRewardGenerator _bonusRewardGenerator;
        private readonly BonusRouletteSpinner _bonusRouletteSpinner;
        private readonly DailyRewardPopupStateController _dailyRewardPopupController;
        private readonly IAudioService _audioService;

        private DailySpinScreen _screen;
        
        private List<BonusReward> _bonusRewards;

        public BonusStateController(ILogger logger, IUiService uiService, 
            LoginService loginService, BonusRewardGenerator bonusRewardGenerator, 
            BonusRouletteSpinner bonusRouletteSpinner, DailyRewardPopupStateController dailyRewardPopupController,
            IAudioService audioService) : base(logger)
        {
            _uiService = uiService;
            _loginService = loginService;
            _bonusRewardGenerator = bonusRewardGenerator;
            _bonusRouletteSpinner = bonusRouletteSpinner;
            _dailyRewardPopupController = dailyRewardPopupController;
            _audioService = audioService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();

            GenerateRewards();
            
            if(!_loginService.CanSpin())
                _screen.DisableSpinning();
            
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.DailySpinScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<DailySpinScreen>(ConstScreens.DailySpinScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<MainScreenStateController>();
            _screen.OnSpinPressed += ProcessSpin;
        }

        private void GenerateRewards()
        {
            _bonusRewards = _bonusRewardGenerator.GenerateRewards(_screen.Slots.ToList());
            for(int i = 0; i < _bonusRewards.Count; i++)
                _screen.Slots[i].Initialize(_bonusRewards[i]);
        }

        private async void ProcessSpin()
        {
            _screen.DisableInteractibleObjects();
            
            _audioService.PlaySound(ConstAudio.RouletteSound);
            int rewardId = Random.Range(0, _bonusRewards.Count);
            BonusReward reward = _bonusRewards[rewardId];
            
            await _bonusRouletteSpinner.Spin(_screen.RouletteTransform, rewardId);
            
            _loginService.RecordSpinDate();

            _dailyRewardPopupController.SetBonusReward(reward);
            _dailyRewardPopupController.Enter().Forget();
        }
    }
}
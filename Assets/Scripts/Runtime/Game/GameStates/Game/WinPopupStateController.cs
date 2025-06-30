using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.UI.Popup;
using Runtime.Game.GameStates.Game.Menu;
using Runtime.Game.Leaderboard;
using Runtime.Game.Services.UI;
using UnityEngine;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

public class WinPopupStateController : StateController
{
    private readonly IUiService _uiService;
    private readonly UserProgressService _progressService;
    private readonly GameplayData _gameplayData;
    
    public WinPopupStateController(ILogger logger, IUiService uiService, UserProgressService userProgressService, GameplayData gameplayData) : base(logger)
    {
        _uiService = uiService;
        _progressService = userProgressService;
        _gameplayData = gameplayData;
    }

    public override async UniTask Enter(CancellationToken cancellationToken = default)
    {
        Time.timeScale = 0;
        WinPopup popup = await _uiService.ShowPopup(ConstPopups.WinPopup) as WinPopup;

        popup.SetData(_gameplayData.ClearReward, _gameplayData.ClearRating, _gameplayData.Steps);
        
        popup.OnHomeButtonPressed += async () =>
        {
            Time.timeScale = 1;
            popup.DestroyPopup();
            await GoTo<MainScreenStateController>();
        };
        
        popup.OnRetryButtonPressed += async () =>
        {
            Time.timeScale = 1;
            popup.DestroyPopup();
            await GoTo<GameplayStateController>();
        };
        
        popup.OnNextButtonPressed += async () =>
        {
            Time.timeScale = 1;
            popup.DestroyPopup();

            if (_progressService.NextLevelExists())
                _gameplayData.LevelId++;
            
            await GoTo<GameplayStateController>();
        };
    }
}

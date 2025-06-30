using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

public class ClearRatingCalculator
{
    private readonly GameplayData _gameplayData;
    private readonly LevelDataProvider _levelDataProvider;

    public ClearRatingCalculator(GameplayData gameplayData, LevelDataProvider levelDataProvider)
    {
        _gameplayData = gameplayData;
        _levelDataProvider = levelDataProvider;
    }

    public int CalculateClearRating()
    {
        var levelConfig = _levelDataProvider.GetCurrentMapConfig();
        
        int steps = _gameplayData.Steps;

        if (steps <= levelConfig.ThreeStarCondition)
            return 3;
        
        if(steps <= levelConfig.TwoStarCondition)
            return 2;
        
        return 1;
    }
}

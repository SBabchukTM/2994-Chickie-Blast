using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCalculator
{
    private const int BaseClearReward = 100;
    private const int RewardPerLevel = 20;
    private const int PenaltyPerStep = 2;

    private readonly GameplayData _gameplayData;

    public RewardCalculator(GameplayData gameplayData)
    {
        _gameplayData = gameplayData;
    }

    public int CalculateReward()
    {
        int levelId = _gameplayData.LevelId;
        int steps = _gameplayData.Steps;

        float levelReward = levelId * RewardPerLevel;

        float stepPenalty = steps * PenaltyPerStep;

        float finalReward = Mathf.Clamp(levelReward - stepPenalty, 0, 999) + BaseClearReward;

        return Mathf.Max(0, Mathf.RoundToInt(finalReward));
    }
}

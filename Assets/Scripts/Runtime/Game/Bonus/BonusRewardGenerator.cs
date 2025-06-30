using System;
using System.Collections.Generic;

public class BonusRewardGenerator
{
    private static Random random = new Random();
    
    public List<BonusReward> GenerateRewards(List<BonusRouletteSlot> bonusRouletteSlots)
    {
        List<BonusReward> bonusRewards = new List<BonusReward>();

        foreach (var slot in bonusRouletteSlots)
        {
            RewardType rewardType = (RewardType)random.Next(0, 2);
            int rewardValue;

            if (rewardType == RewardType.Coin)
            {
                int[] coinValues = { 100, 250, 500 };
                rewardValue = coinValues[random.Next(coinValues.Length)];
            }
            else
            {
                rewardValue = random.Next(0, 2);
            }

            bonusRewards.Add(new BonusReward
            {
                RewardType = rewardType,
                RewardValue = rewardValue
            });
        }

        return bonusRewards;
    }
}

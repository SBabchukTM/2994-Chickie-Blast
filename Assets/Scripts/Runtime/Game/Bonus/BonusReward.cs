using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusReward
{
    public RewardType RewardType;
    public int RewardValue;
}

public enum RewardType
{
    Coin,
    Item
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusRouletteSlot : MonoBehaviour
{
    [SerializeField] private GameObject _coinParent;
    [SerializeField] private TextMeshProUGUI _coinText;
    
    [SerializeField] private GameObject _itemParent;
    [SerializeField] private Image _itemImage;
    [SerializeField] private Sprite _chickSprite;
    [SerializeField] private Sprite _coneSprite;
    
    private BonusReward _bonusReward;
    
    public void Initialize(BonusReward bonusReward)
    {
        _bonusReward = bonusReward;

        if (bonusReward.RewardType == RewardType.Coin)
        {
            _coinParent.SetActive(true);
            _coinText.text = bonusReward.RewardValue.ToString();
        }

        if (bonusReward.RewardType == RewardType.Item)
        {
            _itemParent.SetActive(true);
            _itemImage.sprite = bonusReward.RewardValue == 0? _chickSprite : _coneSprite;
        }
    }
}

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class DailyItemPopup : BasePopup
    {
        [SerializeField] private Button _claimButton;
        
        [SerializeField] private GameObject _coinsParent;
        [SerializeField] private GameObject _itemParent;
        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private Image _itemImage;
        [SerializeField] private Sprite _coneSprite;
        [SerializeField] private Sprite _chickSprite;
        
        public event Action OnClaimed;
        
        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _claimButton.onClick.AddListener(() => OnClaimed?.Invoke());
            return base.Show(data, cancellationToken);
        }

        public void SetData(BonusReward reward)
        {
            if (reward.RewardType == RewardType.Coin)
            {
                _coinsParent.SetActive(true);
                _coinsText.text = reward.RewardValue.ToString();
            }

            if (reward.RewardType == RewardType.Item)
            {
                _itemParent.SetActive(true);
                _itemImage.sprite = reward.RewardValue == 0 ? _chickSprite : _coneSprite;
            }
        }
    }
}
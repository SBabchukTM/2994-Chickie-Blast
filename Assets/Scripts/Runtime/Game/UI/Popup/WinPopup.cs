using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class WinPopup : BasePopup
    {
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private TextMeshProUGUI _stepsText;
        [SerializeField] private Image[] _stars;
        [SerializeField] private Sprite _starSprite;
        
        public event Action OnHomeButtonPressed;
        public event Action OnRetryButtonPressed;
        public event Action OnNextButtonPressed;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _homeButton.onClick.AddListener(() => OnHomeButtonPressed?.Invoke());
            _retryButton.onClick.AddListener(() => OnRetryButtonPressed?.Invoke());
            _nextButton.onClick.AddListener(() => OnNextButtonPressed?.Invoke());
            
            return base.Show(data, cancellationToken);
        }

        public void SetData(int reward, int stars, int steps)
        {
            _rewardText.text = reward.ToString();
            _stepsText.text = steps.ToString();

            for (int i = 0; i < stars; i++)
                _stars[i].sprite = _starSprite;
        }
    }
}
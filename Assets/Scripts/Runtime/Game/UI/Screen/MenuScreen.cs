using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class MenuScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _profileButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private Button _privacyButton;
        [SerializeField] private Button _termsOfUseButton;
        
        public event Action OnBackPressed;
        public event Action OnProfilePressed;
        public event Action OnSettingsPressed;
        public event Action OnLeaderboardPressed;
        public event Action OnPrivacyPolicyPressed;
        public event Action OnTermsOfUsePressed;

        public void Initialize()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            _profileButton.onClick.AddListener(() => OnProfilePressed?.Invoke());
            _settingsButton.onClick.AddListener(() => OnSettingsPressed?.Invoke());
            _leaderboardButton.onClick.AddListener(() => OnLeaderboardPressed?.Invoke());
            _privacyButton.onClick.AddListener(() => OnPrivacyPolicyPressed?.Invoke());
            _termsOfUseButton.onClick.AddListener(() => OnTermsOfUsePressed?.Invoke());
        }
    }
}
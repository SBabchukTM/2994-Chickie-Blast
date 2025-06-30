using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class MainScreen : UiScreen
    {
        [SerializeField] private Button _helpButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _bonusButton;
        
        public event Action OnHelpPressed;
        public event Action OnPlayPressed;
        public event Action OnMenuPressed;
        public event Action OnShopPressed;
        public event Action OnBonusPressed;

        public void Initialize()
        {
            _helpButton.onClick.AddListener(() => OnHelpPressed?.Invoke());
            _playButton.onClick.AddListener(() => OnPlayPressed?.Invoke());
            _menuButton.onClick.AddListener(() => OnMenuPressed?.Invoke());
            _shopButton.onClick.AddListener(() => OnShopPressed?.Invoke());
            _bonusButton.onClick.AddListener(() => OnBonusPressed?.Invoke());
        }
    }
}
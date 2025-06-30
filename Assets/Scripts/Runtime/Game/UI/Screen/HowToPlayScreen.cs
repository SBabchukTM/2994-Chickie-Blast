using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class HowToPlayScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _menuButton;
        
        public event Action OnBackPressed;
        public event Action OnMenuPressed;

        public void Initialize()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            _menuButton.onClick.AddListener(() => OnMenuPressed?.Invoke());
        }
    }
}
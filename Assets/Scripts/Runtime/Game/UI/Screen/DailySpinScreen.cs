using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class DailySpinScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _spinButton;
        [SerializeField] private RectTransform _rouletteTransform;
        [SerializeField] private BonusRouletteSlot[] _slots;
        [SerializeField] private GameObject _errorText;
        
        public BonusRouletteSlot[] Slots => _slots;
        public RectTransform RouletteTransform => _rouletteTransform;
        
        public event Action OnBackPressed;
        public event Action OnSpinPressed;

        public void Initialize()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            _spinButton.onClick.AddListener(() => OnSpinPressed?.Invoke());
        }

        public void DisableSpinning()
        {
            _spinButton.gameObject.SetActive(false);
            _errorText.gameObject.SetActive(true);
        }

        public void DisableInteractibleObjects()
        {
            _backButton.interactable = false;
            _spinButton.interactable = false;
        }
    }
}
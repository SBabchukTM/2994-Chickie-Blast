using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class ShopScreen : UiScreen
    {
        private const float AnimTime = 1;
        
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private ShopSection _skinsButton;
        [SerializeField] private ShopSection _itemsButton;
        [SerializeField] private ShopItemDisplay _chickItemDisplay;
        [SerializeField] private ShopItemDisplay _coneItemDisplay;
        [SerializeField] private RectTransform _errorRect;
        
        public event Action OnBackPressed;
        public event Action OnMenuPressed;
        
        public event Action OnChickPressed;
        public event Action OnConePressed;

        public void Initialize(List<ShopItemDisplay> shopItems, int chickPrice, int conePrice)
        {
            foreach (var item in shopItems)
            {
                item.transform.SetParent(_skinsButton.RectTransform, false);
            }

            _chickItemDisplay.Initialize(chickPrice);
            _coneItemDisplay.Initialize(conePrice);
            
            Subscribe();
        }

        private void Subscribe()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            _menuButton.onClick.AddListener(() => OnMenuPressed?.Invoke());
            
            _skinsButton.Button.onClick.AddListener(() =>
            {
                _skinsButton.Enable(true);
                _itemsButton.Enable(false);
            });
            
            _itemsButton.Button.onClick.AddListener(() =>
            {
                _skinsButton.Enable(false);
                _itemsButton.Enable(true);
            });
            
            _chickItemDisplay.OnBuyItemPressed += () => OnChickPressed?.Invoke();
            _coneItemDisplay.OnBuyItemPressed += () => OnConePressed?.Invoke();
        }

        public void ShowError()
        {
            _errorRect.DOKill();
            
            Sequence sequence = DOTween.Sequence().SetLink(gameObject);

            sequence.Append(_errorRect.DOAnchorPosY(-500, AnimTime));
            sequence.AppendInterval(1f);
            sequence.Append(_errorRect.DOAnchorPosY(0, AnimTime));

            sequence.Play();
        }
    }
    
    [Serializable]
    public class ShopSection
    {
        public Button Button;
        public RectTransform RectTransform;

        public void Enable(bool enable)
        {
            RectTransform.gameObject.SetActive(enable);
                
            Color color = Color.white;
                
            color.a = enable ? 1f : 0.5f;
                
            Button.image.color = color;
        }
    }
}
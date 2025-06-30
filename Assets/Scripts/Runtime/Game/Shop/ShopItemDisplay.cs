using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDisplay : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _statusText;
    
    public event Action<SkinConfig> OnPressed;
    public event Action OnBuyItemPressed;
    
    public void Initialize(SkinConfig skinConfig)
    {
        _image.sprite = skinConfig.ItemSprite;
        _priceText.text = skinConfig.Price.ToString();
        
        _buyButton.onClick.AddListener(() => OnPressed?.Invoke(skinConfig));
    }

    public void Initialize(int price)
    {
        _priceText.text = price.ToString();
        _buyButton.onClick.AddListener(() => OnBuyItemPressed?.Invoke());
    }

    public void SetStatus(string status)
    {
        _statusText.text = status;
    }
}

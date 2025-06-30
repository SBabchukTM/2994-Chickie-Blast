using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Shop;
using TMPro;
using UnityEngine;
using Zenject;

public class BalanceDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _balanceText;

    private InventoryService _inventoryService;
    
    [Inject]
    private void Construct(InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
        
        _balanceText.text = _inventoryService.GetBalance().ToString();
        _inventoryService.OnBalanceChanged += UpdateBalance;
    }

    private void OnDestroy()
    {
        _inventoryService.OnBalanceChanged -= UpdateBalance;
    }

    private void UpdateBalance(int balance)
    {
        _balanceText.text = balance.ToString();
    }
}

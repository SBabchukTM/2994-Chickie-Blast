using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Services.SettingsProvider;
using Runtime.Game.Services.UserData;
using UnityEngine;
using Zenject;

public class ShopItemsFactory : IInitializable
{
    private readonly UserDataService _userDataService;
    private readonly IAssetProvider _assetProvider;
    private readonly ISettingProvider _settingProvider;
    private readonly GameObjectFactory _gameObjectFactory;

    private GameObject _prefab;

    public ShopItemsFactory(UserDataService userDataService, IAssetProvider assetProvider,
        ISettingProvider settingProvider, GameObjectFactory gameObjectFactory)
    {
        _userDataService = userDataService;
        _assetProvider = assetProvider;
        _settingProvider = settingProvider;
        _gameObjectFactory = gameObjectFactory;
    }
    
    public async void Initialize()
    {
        _prefab = await _assetProvider.Load<GameObject>(ConstPrefabs.ShopItemPrefab);
    }

    public List<ShopItemDisplay> CreateShopItems()
    {
        List<ShopItemDisplay> shopItems = new List<ShopItemDisplay>();
        
        var shopConfig = _settingProvider.Get<ShopConfig>();

        for (int i = 0; i < shopConfig.Skins.Count; i++)
        {
            var skin = shopConfig.Skins[i];

            var display = _gameObjectFactory.Create<ShopItemDisplay>(_prefab);
            display.Initialize(skin);
            shopItems.Add(display);
        }
        
        return shopItems;
    }
}

using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Shop;
using UnityEngine;

public class ChickenSpawner
{
    private readonly ISettingProvider _settingProvider;
    private readonly GameObjectFactory _factory;
    private readonly InventoryService _inventoryService;

    private GameObject _prefab;
    
    public ChickenSpawner(ISettingProvider settingProvider, GameObjectFactory factory, InventoryService inventoryService)
    {
        _settingProvider = settingProvider;
        _factory = factory;
        _inventoryService = inventoryService;
    }
    

    public Transform SpawnChicken()
    {
        var chicken = _factory.Create(_settingProvider.Get<ChickenPrefabsConfig>().prefabs[_inventoryService.GetUsedSkinId()]);
        return chicken.transform;
    }
}

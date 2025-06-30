using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

namespace Runtime.Game.Shop
{
    public class SkinService
    {
        private readonly InventoryService _inventoryService;
        private readonly ISettingProvider _settingProvider;

        public SkinService(InventoryService inventoryService, ISettingProvider settingProvider)
        {
            _inventoryService = inventoryService;
            _settingProvider = settingProvider;
        }

        public Sprite GetUsedSkin()
        {
            var shopConfig = _settingProvider.Get<ShopConfig>();
            return shopConfig.Skins[_inventoryService.GetUsedSkinId()].ItemSprite;
        }
    }
}
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Services.UserData;

namespace Runtime.Game.Shop
{
    public class ShopService
    {
        private readonly UserDataService _userDataService;
        private readonly InventoryService _inventoryService;
        private readonly ISettingProvider _settingProvider;

        public ShopService(UserDataService userDataService, InventoryService inventoryService,
            ISettingProvider settingProvider)
        {
            _userDataService = userDataService;
            _inventoryService = inventoryService;
            _settingProvider = settingProvider;
        }

        public void PurchaseSkin(SkinConfig config)
        {
            _inventoryService.AddBalance(-config.Price);
            _inventoryService.AddPurchasedSkin(GetItemID(config));
        }

        public void PurchaseCone()
        {
            _inventoryService.AddBalance(-GetConePrice());
            _inventoryService.AddCone();
        }

        public void PurchaseSpawnChick()
        {
            _inventoryService.AddBalance(-GetChickPrice());
            _inventoryService.AddChick();
        }
        
        public bool IsItemPurchased(SkinConfig config)
        {
            var purchasedItems = _inventoryService.GetPurchasedSkinsList();
            return purchasedItems.Contains(GetItemID(config));
        }
        
        public int GetConePrice() => GetShopConfig().ConePrice;
        public int GetChickPrice() => GetShopConfig().SpawnChicksPrice;
        
        public bool CanPurchaseSkin(SkinConfig config) => _inventoryService.GetBalance() >= config.Price;
        public bool CanPurchaseCone() => _inventoryService.GetBalance() >= GetConePrice();
        public bool CanPurchaseChickSpawn() => _inventoryService.GetBalance() >= GetChickPrice();

        public bool IsSelected(SkinConfig config) => _inventoryService.GetUsedSkinId() == GetItemID(config);

        public int GetItemID(SkinConfig config) => GetShopConfig().Skins.IndexOf(config);
        private ShopConfig GetShopConfig() => _settingProvider.Get<ShopConfig>();
    }
}
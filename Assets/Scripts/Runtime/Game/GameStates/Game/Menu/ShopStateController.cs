using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.Logger;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UI;
using Runtime.Game.Shop;
using Runtime.Game.UI.Screen;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class ShopStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly ShopItemsFactory _shopItemsFactory;
        private readonly ShopService _shopService;
        private readonly InventoryService _inventoryService;
        private readonly IAudioService _audioService;

        private ShopScreen _screen;
        
        private List<ShopItemDisplay> _shopItems;

        public ShopStateController(ILogger logger, IUiService uiService, 
            ShopItemsFactory shopItemsFactory, ShopService shopService,
            InventoryService inventoryService, IAudioService audioService) : base(logger)
        {
            _uiService = uiService;
            _shopItemsFactory = shopItemsFactory;
            _shopService = shopService;
            _inventoryService = inventoryService;
            _audioService = audioService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateSkinDisplays();
            CreateScreen();
            SubscribeToEvents();
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.ShopScreen);
        }

        private void CreateSkinDisplays()
        {
            _shopItems = _shopItemsFactory.CreateShopItems();
            UpdateItemsDisplay();
            foreach (var item in _shopItems)
                item.OnPressed += ProcessItem;
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<ShopScreen>(ConstScreens.ShopScreen);
            _screen.Initialize(_shopItems, _shopService.GetChickPrice(), _shopService.GetConePrice());
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<MainScreenStateController>();
            _screen.OnMenuPressed += async () => await GoTo<MenuStateController>();

            _screen.OnChickPressed += () =>
            {
                if (!_shopService.CanPurchaseChickSpawn())
                {
                    _audioService.PlaySound(ConstAudio.ErrorSound);
                    _screen.ShowError();
                    return;
                }

                _audioService.PlaySound(ConstAudio.PurchaseSound);
                _shopService.PurchaseSpawnChick();
            };
            
            _screen.OnConePressed += () =>
            {
                if (!_shopService.CanPurchaseCone())
                {
                    _audioService.PlaySound(ConstAudio.ErrorSound);
                    _screen.ShowError();
                    return;
                }

                _audioService.PlaySound(ConstAudio.PurchaseSound);
                _shopService.PurchaseCone();
            };
        }

        private void ProcessItem(SkinConfig config)
        {
            if (_shopService.IsItemPurchased(config))
            {
                _audioService.PlaySound(ConstAudio.PurchaseSound);
                _inventoryService.SetUsedSkinId(_shopService.GetItemID(config));
            }
            else if (_shopService.CanPurchaseSkin(config))
            {
                _audioService.PlaySound(ConstAudio.PurchaseSound);
                _shopService.PurchaseSkin(config);
            }
            else
            {
                _audioService.PlaySound(ConstAudio.ErrorSound);
                _screen.ShowError();
            }

            UpdateItemsDisplay();
        }

        private void UpdateItemsDisplay()
        {
            for (int i = 0; i < _shopItems.Count; i++)
            {
                var item = _shopItems[i];
                var itemId = i;
                
                item.SetStatus(GetItemStatus(itemId));
            }
        }

        private string GetItemStatus(int id)
        {
            if (_inventoryService.GetUsedSkinId() == id)
                return "USED";

            if (_inventoryService.GetPurchasedSkinsList().Contains(id))
                return "USE";

            return "BUY";
        }
    }
}
using System;
using System.Collections.Generic;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;

namespace Runtime.Game.Shop
{
    public class InventoryService
    {
        private readonly UserDataService _userDataService;

        public event Action<int> OnBalanceChanged;
        public event Action<int> OnConeUsed;
        public event Action<int> OnChickUsed;
        
        public InventoryService(UserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        public void SetUsedSkinId(int skinId) => GetInventoryData().UsedSkinID = skinId;
        public int GetBalance() => GetInventoryData().Balance;
        public List<int> GetPurchasedSkinsList() => GetInventoryData().PurchasedSkins;
        public int GetUsedSkinId() => GetInventoryData().UsedSkinID;
        public int GetConesAmount() => GetInventoryData().ConesAmount;
        public int GetSpawnChicksAmount() => GetInventoryData().SpawnChicksAmount;

        public void AddBalance(int amount)
        {
            GetInventoryData().Balance += amount;
            OnBalanceChanged?.Invoke(GetInventoryData().Balance);
        }
        
        public void UseCone()
        {
            GetInventoryData().ConesAmount--;
            OnConeUsed?.Invoke(GetConesAmount());
        }
        
        public void UseChick()
        {
            GetInventoryData().SpawnChicksAmount--;
            OnChickUsed?.Invoke(GetSpawnChicksAmount());
        }

        public void AddChick() => GetInventoryData().SpawnChicksAmount++;
        public void AddCone() => GetInventoryData().ConesAmount++;
        
        public void AddPurchasedSkin(int skinId)
        {
            GetInventoryData().PurchasedSkins.Add(skinId);
            SetUsedSkinId(skinId);
        }
        
        private InventoryData GetInventoryData() => _userDataService.GetUserData().InventoryData;
    }
}
using System;
using System.Collections.Generic;

namespace Runtime.Game.Services.UserData.Data
{
    [Serializable]
    public class InventoryData
    {
        public int Balance = 0;
        public List<int> PurchasedSkins = new(){0};
        public int UsedSkinID = 0;
        
        public int ConesAmount = 0;
        public int SpawnChicksAmount = 0;
    }
}
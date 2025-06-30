using System;
using System.Collections.Generic;
using Runtime.Game.Services.UserData.Data;
using Runtime.Game.UserAccountSystem;
using UnityEngine.Serialization;

namespace Runtime.Game.Services.UserData
{
    [Serializable]
    public class UserData
    {
        public List<GameSessionData> GameSessionData = new List<GameSessionData>();
        public SettingsData SettingsData = new SettingsData();
        public GameData GameData = new GameData();
        public InventoryData InventoryData = new InventoryData();
        public LoginData LoginData = new LoginData();
        public ProfileData ProfileData = new ProfileData();
        public UserProgressData UserProgressData = new UserProgressData();
    }
}
using System;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Services.UserData;
using UnityEngine;

namespace Runtime.Game.UserAccountSystem
{
    public class ProfileService
    {
        private readonly UserDataService _userDataService;
        private readonly ImageProcessorHelper _imageProcessorHelper;
    
        public ProfileService(UserDataService userDataService, 
            ImageProcessorHelper imageProcessorHelper)
        {
            _userDataService = userDataService;
            _imageProcessorHelper = imageProcessorHelper;
        }
    
        public ProfileData GetAccountDataCopy()
        {
            return _userDataService.GetUserData().ProfileData.Copy();
        }

        public void SaveAccountData(ProfileData modifiedData)
        {
            var origData = _userDataService.GetUserData().ProfileData;

            foreach (var field in typeof(ProfileData).GetFields())
                field.SetValue(origData, field.GetValue(modifiedData));

            _userDataService.SaveUserData();
        }

        public Sprite GetUsedAvatarSprite()
        {
            if (!AvatarExists())
            {
                return null;
            }
            
            return _imageProcessorHelper.CreateSpriteFromBase64(GetAvatarBase64());
        }

        public string ConvertToBase64(Sprite sprite, int maxSize = 512) =>
            _imageProcessorHelper.ConvertToBase64(sprite, maxSize);


        private bool AvatarExists() => _userDataService.GetUserData().ProfileData.AvatarBase64 != String.Empty;
        
        private string GetAvatarBase64() => _userDataService.GetUserData().ProfileData.AvatarBase64;
    }
}

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData;
using Runtime.Game.UI.Screen;
using Runtime.Game.UserAccountSystem;
using UnityEngine;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class ProfileStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly AvatarSelectHelper _avatarSelectHelper;
        private readonly ProfileService _profileService;

        private ProfileScreen _screen;

        private ProfileData _modifiedData;

        public ProfileStateController(ILogger logger, IUiService uiService, 
            AvatarSelectHelper avatarSelectHelper, ProfileService profileService) : base(logger)
        {
            _uiService = uiService;
            _avatarSelectHelper = avatarSelectHelper;
            _profileService = profileService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            _modifiedData = _profileService.GetAccountDataCopy();
            CreateScreen();
            SubscribeToEvents();
            
            _screen.SetAvatar(_profileService.GetUsedAvatarSprite());
            _screen.SetName(_modifiedData.Username);
            
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.ProfileScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<ProfileScreen>(ConstScreens.ProfileScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<MenuStateController>();
            _screen.OnSavePressed += async () =>
            {
                Save();
                await GoTo<MenuStateController>();
            };
            
            _screen.OnNameChanged += ValidateName;
            _screen.OnAvatarPressed += SelectAvatar;
        }

        private void Save() => _profileService.SaveAccountData(_modifiedData);

        private void ValidateName(string name)
        {
            if (name.Length < 2 || !Char.IsLetter(name[0]))
            {
                _screen.SetName(_modifiedData.Username);
                return;
            }
            
            _modifiedData.Username = name;
        }

        private async void SelectAvatar()
        {
            Sprite newAvatar = await _avatarSelectHelper.SelectAvatar(512);
            if (newAvatar)
            {
                _screen.SetAvatar(newAvatar);
                _modifiedData.AvatarBase64 = _profileService.ConvertToBase64(newAvatar);
            }
        }
    }
}
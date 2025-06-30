using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.Logger;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;
using Runtime.Game.UI.Screen;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class SettingsStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly UserDataService _userDataService;
        private readonly IAudioService _audioService;

        private SettingsScreen _screen;

        private float _origSound;
        private float _origMusic;
        
        private float _newSound;
        private float _newMusic;

        public SettingsStateController(ILogger logger, IUiService uiService, UserDataService userDataService, IAudioService audioService) : base(logger)
        {
            _uiService = uiService;
            _userDataService = userDataService;
            _audioService = audioService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.SettingsScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<SettingsScreen>(ConstScreens.SettingsScreen);
            _screen.Initialize(_userDataService.GetUserData().SettingsData);
            _screen.ShowAsync().Forget();
            
            _origSound = _userDataService.GetUserData().SettingsData.SoundVolume;
            _origMusic = _userDataService.GetUserData().SettingsData.MusicVolume;
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () =>
            {
                _audioService.SetVolume(AudioType.Sound, _origSound);
                _audioService.SetVolume(AudioType.Music, _origMusic);
                
                await GoTo<MenuStateController>();
            };
            
            _screen.OnSavePressed += async () =>
            {
                SettingsData settingsData = _userDataService.GetUserData().SettingsData;
                settingsData.SoundVolume = _newSound;
                settingsData.MusicVolume = _newMusic;
                await GoTo<MenuStateController>();
            };

            _screen.OnMusicVolumeChanged += OnChangeMusicVolume;
            _screen.OnSoundVolumeChanged += OnChangeSoundVolume;
        }
        
        private void OnChangeSoundVolume(float value)
        {
            _audioService.SetVolume(AudioType.Sound, value);
            _newSound = value;
        }

        private void OnChangeMusicVolume(float value)
        {
            _audioService.SetVolume(AudioType.Music, value);
            _newMusic = value;
        }
    }
}
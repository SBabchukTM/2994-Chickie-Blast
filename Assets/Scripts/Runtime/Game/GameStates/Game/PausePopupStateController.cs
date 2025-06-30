using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.GameStateMachine;
using Runtime.Core.UI.Popup;
using Runtime.Game.GameStates.Game.Menu;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData;
using UnityEngine;
using AudioType = Runtime.Core.Audio.AudioType;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

public class PausePopupStateController : StateController
{
    private readonly IUiService _uiService;
    private readonly UserDataService _userDataService;
    private readonly IAudioService _audioService;
    
    public PausePopupStateController(ILogger logger, IUiService uiService, UserDataService userDataService, IAudioService audioService) : base(logger)
    {
        _uiService = uiService;
        _userDataService = userDataService;
        _audioService = audioService;
    }

    public override async UniTask Enter(CancellationToken cancellationToken = default)
    {
        Time.timeScale = 0;
        PausePopup popup = await _uiService.ShowPopup(ConstPopups.PausePopup) as PausePopup;

        var settings = _userDataService.GetUserData().SettingsData;
        popup.SetData(settings.MusicVolume, settings.SoundVolume);
        
        popup.OnContPressed += () =>
        {
            Time.timeScale = 1;
            popup.DestroyPopup();
        };

        popup.OnHomePressed += async () =>
        {
            Time.timeScale = 1;
            popup.DestroyPopup();
            await GoTo<MainScreenStateController>();
        };

        popup.OnMusicChanged += (volume) =>
        {
            _audioService.SetVolume(AudioType.Music, volume);
            settings.MusicVolume = volume;
        };
        
        popup.OnSoundChanged += (volume) =>
        {
            _audioService.SetVolume(AudioType.Sound, volume);
            settings.SoundVolume = volume;
        };
    }
}

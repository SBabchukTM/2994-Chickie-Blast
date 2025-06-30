using Runtime.Core.UI.Data;
using Runtime.Game.Services.UserData.Data;

namespace Runtime.Game.UI.Popup.Data
{
    public class SettingsPopupData : BasePopupData
    {
        private bool _isSoundVolume;
        private bool _isMusicVolume;

        public bool IsSoundVolume => _isSoundVolume;
        public bool IsMusicVolume => _isMusicVolume;

        public SettingsPopupData(bool isSoundVolume, bool isMusicVolume)
        {
            _isSoundVolume = isSoundVolume;
            _isMusicVolume = isMusicVolume;
        }
    }
}
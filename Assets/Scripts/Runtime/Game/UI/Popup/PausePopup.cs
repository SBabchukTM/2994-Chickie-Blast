using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class PausePopup : BasePopup
    {
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _contButton;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundSlider;

        public event Action OnHomePressed;
        public event Action OnContPressed;
        public event Action<float> OnMusicChanged;
        public event Action<float> OnSoundChanged;
        
        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _homeButton.onClick.AddListener(() => OnHomePressed?.Invoke());
            _contButton.onClick.AddListener(() => OnContPressed?.Invoke());
            _musicSlider.onValueChanged.AddListener((value) => OnMusicChanged?.Invoke(value));
            _soundSlider.onValueChanged.AddListener((value) => OnSoundChanged?.Invoke(value));
            return base.Show(data, cancellationToken);
        }

        public void SetData(float music, float sound)
        {
            _musicSlider.value = music;
            _soundSlider.value = sound;
        }
    }
}
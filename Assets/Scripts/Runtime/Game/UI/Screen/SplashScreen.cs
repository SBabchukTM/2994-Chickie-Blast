using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class SplashScreen : UiScreen
    {
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private float animationDuration = 3f;
        
        public override async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            await SimulateLoadingAsync(animationDuration, cancellationToken);
            await base.HideAsync(cancellationToken);
        }

        private async UniTask SimulateLoadingAsync(float duration, CancellationToken cancellationToken)
        {
            float elapsed = 0f;
            float progress = 0f;

            while (elapsed < duration)
            {
                cancellationToken.ThrowIfCancellationRequested();

                elapsed += Time.deltaTime;
                float targetProgress = Mathf.Clamp01(elapsed / duration);

                float jitter = Random.Range(-0.01f, 0.01f);
                progress = Mathf.Clamp01(targetProgress + jitter);

                _progressBar.value = progress;
                _progressText.text = Mathf.FloorToInt(progress * 100f) + "%";

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }

            _progressBar.value = 1f;
            _progressText.text = "100%";
        }
    }
}
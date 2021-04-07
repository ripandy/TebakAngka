using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Kassets.Utilities;
using Kassets.VariableSystem;
using UnityEngine;

namespace Feature.LoadingScreen.Tests
{
    public class TestLoadingFade : MonoBehaviour
    {
        [SerializeField] private FloatVariable _fadeDuration;
        [SerializeField] private BoolVariable _isLoading;
        [SerializeField] private BoolVariable _shouldFade;
        [SerializeField] private KeyCode _triggerKey = KeyCode.L;
        [SerializeField] private float _simulatedLoadingTime = 2f;
        
        private void Start()
        {
            this.Magenta($"[{GetType().Name}] Press {_triggerKey} to start fade-in and fade-out.");

            var token = this.GetCancellationTokenOnDestroy();
            UniTaskAsyncEnumerable.EveryValueChanged(this, fade => Input.GetKeyDown(_triggerKey))
                .SubscribeAwait(async _ => await DummyLoadingProgress(token), token);
        }

        private async UniTask DummyLoadingProgress(CancellationToken token)
        {
            _shouldFade.Value = true;
            _isLoading.Value = true;

            if (_shouldFade)
                await UniTask.Delay(Mathf.FloorToInt(_fadeDuration.Value * 1000), cancellationToken: token);

            var duration = _simulatedLoadingTime;
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: token);

            _isLoading.Value = false;
        }
    }
}
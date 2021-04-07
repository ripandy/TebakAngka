using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Kassets.VariableSystem;
using UnityEngine;

namespace Feature.LoadingScreen
{
    public abstract class BaseFadeHandler : MonoBehaviour
    {
        [SerializeField] private FloatVariable _fadeDuration;
        [SerializeField] private LoadingStateVariable _loadingState;

        private void Start() => Initialize(this.GetCancellationTokenOnDestroy());

        protected virtual void Initialize(CancellationToken token)
        {
            _loadingState.SubscribeAwait(OnLoadingStateChanged, token);
        }

        private async UniTask OnLoadingStateChanged(LoadingStateEnum loadingState)
        {
            if (loadingState == LoadingStateEnum.FadeOut)
                await FadeOut(_fadeDuration);
            else if (loadingState == LoadingStateEnum.FadeIn)
                await FadeIn(_fadeDuration);
        }

        protected abstract UniTask StartFade(float duration, bool fadeIn);
        
        private UniTask FadeIn(float duration) => StartFade(duration, true);

        private UniTask FadeOut(float duration) => StartFade(duration, false);
    }
}
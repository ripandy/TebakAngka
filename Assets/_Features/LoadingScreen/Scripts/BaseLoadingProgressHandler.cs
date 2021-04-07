using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Kassets.VariableSystem;
using UnityEngine;

namespace Feature.LoadingScreen
{
    public abstract class BaseLoadingProgressHandler : MonoBehaviour
    {
        [SerializeField] private LoadingStateVariable _loadingState;
        [SerializeField] private BoolVariable _showLoadingScreen;
        [SerializeField] private FloatVariable _loadingProgress;
        [SerializeField] private GameObject _baseObject;

        private void Start()
        {
            var token = this.GetCancellationTokenOnDestroy();
            _loadingState.Subscribe(OnLoadingStateChanged, token);
            _loadingProgress.Where(_ => _showLoadingScreen.Value).Subscribe(UpdateProgression, token);
        }

        private void OnLoadingStateChanged(LoadingStateEnum loadingState)
        {
            var show = _showLoadingScreen.Value;
            switch (loadingState)
            {
                case LoadingStateEnum.Idle:
                case LoadingStateEnum.FadeOut:
                case LoadingStateEnum.FadeIn:
                    show = false;
                    break;
            }
            _baseObject.SetActive(show);
        }

        protected abstract void UpdateProgression(float progress);
    }
}
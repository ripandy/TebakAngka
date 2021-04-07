using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Feature.ApplicationStateManagement;
using UnityEngine;

namespace Feature.SplashScreen
{

    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] private ApplicationStateVariable _applicationState;
        [SerializeField] protected float splashDuration;

        private void Start() => Initialize().Forget();

        private async UniTaskVoid Initialize()
        {
            var token = this.GetCancellationTokenOnDestroy();
            await ShowSplash(token);
            _applicationState.Value = ApplicationStateEnum.MainMenu;
        }

        protected virtual async UniTask ShowSplash(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(splashDuration), cancellationToken: token);
        }
    }
}

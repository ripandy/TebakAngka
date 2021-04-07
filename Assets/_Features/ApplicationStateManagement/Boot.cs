using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Feature.ApplicationStateManagement
{
    public class Boot : MonoBehaviour
    {
        private void Start() => Initialize().Forget();

        private async UniTaskVoid Initialize()
        {
            var token = this.GetCancellationTokenOnDestroy();
            
            await SceneManager.LoadSceneAsync(SceneNamesEnumCore.Statics.ToString(), LoadSceneMode.Additive).ToUniTask(cancellationToken: token);
            
            await UniTask.NextFrame(token);

            SceneManager.UnloadSceneAsync(SceneNamesEnumCore.Boot.ToString()).ToUniTask(cancellationToken: token).Forget();
        }
    }
}

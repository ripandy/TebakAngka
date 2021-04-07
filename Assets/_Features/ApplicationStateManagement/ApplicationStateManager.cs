using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Feature.LoadingScreen;
using Kassets.VariableSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Feature.ApplicationStateManagement
{
    public class ApplicationStateManager : MonoBehaviour
    {
        [SerializeField] private ApplicationStateVariable _applicationState;
        [SerializeField] private LoadingStateVariable _loadingState;
        [SerializeField] private FloatVariable _loadingProgress;
        [SerializeField] private BoolVariable _showLoadingScreen;
        [SerializeField] private FloatVariable _fadeDuration;
        [SerializeField] private List<ApplicationStateConfig> _stateConfigs = new List<ApplicationStateConfig>();

        // cache
        private readonly Dictionary<ApplicationStateEnum, ApplicationStateConfig> _cachedConfigs = new Dictionary<ApplicationStateEnum, ApplicationStateConfig>();
        private ApplicationStateConfig ActiveState => _cachedConfigs[_applicationState];
        private ApplicationStateConfig PrevState { get; set; }
        
        private void Awake()
        {
            _stateConfigs.ForEach(config =>
            {
                if (!_cachedConfigs.ContainsKey(config.applicationState))
                    _cachedConfigs.Add(config.applicationState, config);
                else
                    UnityEngine.Debug.LogWarning($"[{GetType().Name}] Duplicated config for {config.applicationState}. Config already exist!");
            });
        }


        private void Start()
        {
            var token = this.GetCancellationTokenOnDestroy();
            _applicationState.SubscribeAwait(async (value, ct) => await OnApplicationStateChanged(value, ct), token);
        }

        private async UniTask OnApplicationStateChanged(ApplicationStateEnum newApplicationState, CancellationToken token)
        {
            // necessary to let _applicationState value to be correctly assigned
            await UniTask.NextFrame(token);
            
            _showLoadingScreen.Value = ActiveState.showLoadingScreen;
            var showFade = ActiveState.showFade;

            _loadingProgress.Value = 0; // reset progress
            
            if (showFade)
            {
                _loadingState.Value = LoadingStateEnum.FadeOut;
                await UniTask.Delay(TimeSpan.FromSeconds(_fadeDuration), cancellationToken: token);
            }
            
            // unload scenes
            _loadingState.Value = LoadingStateEnum.UnloadScenes;
            await UnloadPreviousState(token);
            await ReleaseResources(token);
            
            // load scenes
            _loadingState.Value = LoadingStateEnum.LoadScenes;
            await LoadCurrentState(token);
            await ReleaseResources(token);

            // warmup shaders
            _loadingState.Value = LoadingStateEnum.ShaderWarmUp;
            await UniTask.NextFrame(token);
            Shader.WarmupAllShaders();
            
            await UniTask.NextFrame(token);
            
            // wait for load buffer time
            _loadingState.Value = LoadingStateEnum.ProcessOffset;
            await ProcessOffset(token);

            PrevState = _cachedConfigs[newApplicationState];

            if (showFade)
            {
                _loadingState.Value = LoadingStateEnum.FadeIn;
                await UniTask.Delay(TimeSpan.FromSeconds(_fadeDuration), cancellationToken: token);
            }
            
            _loadingState.Value = LoadingStateEnum.Idle;
        }

        private IList<SceneNamesEnum> FilterScenesToUnload()
        {
            var toUnload = new List<SceneNamesEnum>();

            if (PrevState != null)
            {
                foreach (var scene in PrevState.scenes)
                    if (!ActiveState.scenesToKeep.Contains(scene)
                        && SceneManager.GetSceneByName(scene.ToString()).isLoaded)
                        toUnload.Add(scene);
            }

            return toUnload;
        }

        private IList<SceneNamesEnum> FilterScenesToLoad()
        {
            var toLoad = new List<SceneNamesEnum>();

            foreach (var scene in ActiveState.scenes)
                if (!SceneManager.GetSceneByName(scene.ToString()).isLoaded)
                    toLoad.Add(scene);

            return toLoad;
        }

        private async UniTask UnloadPreviousState(CancellationToken token)
        {
            var unloadPercentage = 0.45f; // 0.45 for unload, 0.45 for load, 0.1 for buffer

            var scenes = FilterScenesToUnload();

            if (scenes.Count <= 0)
            {
                Debug.Log($"[{GetType().Name}] No scene to unload..");
                await UniTask.NextFrame(cancellationToken: token);
                _loadingProgress.Value += unloadPercentage;
                return;
            }

            var sceneCountInv = unloadPercentage / scenes.Count;

            foreach (var scene in scenes)
            {
                var progress = new Progress<float>(value =>
                    _loadingProgress.Value = Mathf.Min(0.9f, _loadingProgress.Value + value * sceneCountInv));
                await SceneManager.UnloadSceneAsync(scene.ToString())
                    .ToUniTask(progress, cancellationToken: token);
            }
        }

        private async UniTask LoadCurrentState(CancellationToken token)
        {
            var loadPercentage = 0.45f;

            var scenes = FilterScenesToLoad();

            if (scenes.Count <= 0)
            {
                await UniTask.Yield();
                _loadingProgress.Value += loadPercentage;
                return;
            }

            var sceneCountInv = loadPercentage / scenes.Count;

            foreach (var scene in scenes)
            {
                Debug.Log($"[{GetType().Name}] loading scene {scene}");
                var progress = new Progress<float>(value =>
                    _loadingProgress.Value = Mathf.Min(0.9f, _loadingProgress.Value + value * sceneCountInv));
                await SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive)
                    .ToUniTask(progress, cancellationToken: token);
                
                if (scene == ActiveState.activeScene)
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.ToString()));
            }
        }

        private async UniTask ProcessOffset(CancellationToken token)
        {
            // hold loading progress as long as loadBufferTime. -> loading screen will stay visible.
            var bufferMultiplier = 0.1f / Mathf.Max(0.1f, ActiveState.loadBufferTime);
            while (_loadingProgress.Value < 1)
            {
                await UniTask.NextFrame(cancellationToken: token);
                var delta = Time.deltaTime * bufferMultiplier;
                _loadingProgress.Value += delta;
            }

            _loadingProgress.Value = 1f;
        }

        private async UniTask ReleaseResources(CancellationToken token)
        {
            await Resources.UnloadUnusedAssets().ToUniTask(cancellationToken: token);
            
            await UniTask.NextFrame(token);
            GC.Collect();

            await UniTask.NextFrame(token);
            GC.WaitForPendingFinalizers();

            await UniTask.NextFrame(token);
            GC.Collect();
        }
    }
}
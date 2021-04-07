using UnityEngine;

namespace Feature.ApplicationStateManagement
{
    public enum SceneNamesEnumCore
    {
        Boot,
        Statics
    }

    public enum SceneNamesEnum
    {
        SplashScreen,
        MainMenu,
        Stage,
        Character,
        Gameplay,
        GUI
    }

    [CreateAssetMenu(fileName = "NewApplicationStateConfig", menuName = "ScriptableObjects/ApplicationStateConfig", order = 10)]
    public class ApplicationStateConfig : ScriptableObject
    {
        public ApplicationStateEnum applicationState;

        [Tooltip("Required scenes.")]
        public SceneNamesEnum[] scenes;

        [Tooltip("Scenes to keep (prevent unload) if already loaded.")]
        public SceneNamesEnum[] scenesToKeep;

        [Tooltip("Set which scene to set active. Active scene's lighting settings will be used on other scenes.")]
        public SceneNamesEnum activeScene;

        [Tooltip("Standby time added after load complete. Might be useful to avoid experiencing jitter/lag at first few frames.")]
        public float loadBufferTime;

        [Tooltip("Whether or not to show load progression on state change.")]
        public bool showLoadingScreen;
        
        [Tooltip("Whether or not to fade screen in and out on state change.")]
        public bool showFade;
    }
}
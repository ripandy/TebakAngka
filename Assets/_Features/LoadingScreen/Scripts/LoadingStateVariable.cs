using Kassets.VariableSystem;
using UnityEngine;

namespace Feature.LoadingScreen
{
    public enum LoadingStateEnum
    {
        Idle,
        FadeOut,
        UnloadScenes,
        LoadScenes,
        ShaderWarmUp,
        ProcessOffset,
        FadeIn
    }

    [CreateAssetMenu(fileName = "LoadingState", menuName = "Kassets/Variables/LoadingState")]
    public class LoadingStateVariable : VariableSystemBase<LoadingStateEnum>
    {
    }
}
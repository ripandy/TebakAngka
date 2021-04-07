using Kassets.VariableSystem;
using UnityEngine;

namespace Feature.ApplicationStateManagement
{
    public enum ApplicationStateEnum
    {
        Splash,
        MainMenu,
        GamePlay
    }

    [CreateAssetMenu(fileName = "ApplicationState", menuName = "Kassets/Variables/ApplicationState")]
    public class ApplicationStateVariable : VariableSystemBase<ApplicationStateEnum>
    {
    }
}
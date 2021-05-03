using Kassets;
using Kassets.VariableSystem;
using UnityEngine;

namespace TebakAngka.Gameplay
{
    public enum GameplayStateEnum
    {
        GenerateLevel,
        Answering,
        Answered,
        Win,
        Lose
    }
    
    [CreateAssetMenu(fileName = "GameplayStateVariable", menuName = MenuHelper.DefaultVariableMenu + "GameplayState")]
    public class GameplayStateVariable : VariableSystemBase<GameplayStateEnum>
    {
    }
}
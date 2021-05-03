using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace TebakAngka.Gameplay
{
    public class GameplayStateEngine : MonoBehaviour
    {
        [SerializeField] private GameplayStateVariable _gameState;

        private readonly Dictionary<GameplayStateEnum, IGameplayStateHandler> _gameplayStateHandlers = new Dictionary<GameplayStateEnum, IGameplayStateHandler>();

        private void Start() => Initialize();

        private void PopulateStateHandlers()
        {
            
        }

        private void Initialize()
        {
            var token = this.GetCancellationTokenOnDestroy();
            _gameState.Subscribe(OnStateChanged, token);
        }

        private void OnStateChanged(GameplayStateEnum newState)
        {
            
        }
    }
}
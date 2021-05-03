using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace TebakAngka.Gameplay
{
    public class GameStateEngine : IStartable, IDisposable
    {
        private readonly IList<IGameState> _gameStates;
        private IGameState this[GameStateEnum gameState] => _gameStates[(int) gameState];
        private readonly CancellationTokenSource _lifeTimeCancellationToken = new CancellationTokenSource();

        public GameStateEngine(IList<IGameState> gameStates)
        {
            _gameStates = gameStates;
        }

        public void Start() => RunStateEngine().Forget();

        private async UniTaskVoid RunStateEngine()
        {
            var activeState = this[GameStateEnum.GenerateLevel];
            while (!_lifeTimeCancellationToken.IsCancellationRequested)
            {
                await activeState.OnStateBegan(_lifeTimeCancellationToken.Token);
                var nextState = activeState.OnStateEnded();
                activeState = this[nextState];
            }
        }

        public void Dispose()
        {
            _lifeTimeCancellationToken?.Cancel();
            _lifeTimeCancellationToken?.Dispose();
        }
    }
}
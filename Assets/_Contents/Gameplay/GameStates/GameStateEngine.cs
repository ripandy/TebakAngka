using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace TebakAngka.Gameplay
{
    public class GameStateEngine : IAsyncStartable, IDisposable
    {
        private readonly IReadOnlyList<IGameState> _gameStates;
        private readonly CancellationTokenSource _lifeTimeCancellationTokenSource = new CancellationTokenSource();
        
        private IGameState this[GameStateEnum gameState] => _gameStates[(int) gameState];

        public GameStateEngine(IReadOnlyList<IGameState> gameStates)
        {
            _gameStates = gameStates;
        }

        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            var token = cancellationToken == default ? _lifeTimeCancellationTokenSource.Token : cancellationToken;
            
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f), cancellationToken: token);
            
            RunStateEngine(token).Forget();
        }

        private async UniTaskVoid RunStateEngine(CancellationToken token)
        {
            var nextState = GameStateEnum.GenerateLevel;
            while (!token.IsCancellationRequested)
            {
                var activeState = this[nextState];
                await activeState.OnStateBegan(token);
                nextState = activeState.OnStateEnded();
                await UniTask.Yield();
            }
        }

        public void Dispose()
        {
            _lifeTimeCancellationTokenSource?.Cancel();
            _lifeTimeCancellationTokenSource?.Dispose();
        }
    }
}
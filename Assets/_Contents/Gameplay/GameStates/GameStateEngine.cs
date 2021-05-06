using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks.Linq;
using Kassets.Utilities;
using VContainer.Unity;

namespace TebakAngka.Gameplay
{
    public class GameStateEngine : IStartable, IDisposable
    {
        private readonly IReadOnlyList<IGameState> _gameStates;
        private readonly CancellationTokenSource _lifeTimeCancellationTokenSource = new CancellationTokenSource();
        
        private IGameState this[GameStateEnum gameState] => _gameStates[(int) gameState];

        public GameStateEngine(IReadOnlyList<IGameState> gameStates)
        {
            _gameStates = gameStates;
        }

        public void Start() => RunStateEngine(_lifeTimeCancellationTokenSource.Token);

        private void RunStateEngine(CancellationToken token)
        {
            this.Orange("Running State Engine");
            var nextState = GameStateEnum.GenerateLevel;
            try
            {
                UniTaskAsyncEnumerable.EveryUpdate()
                    .SubscribeAwait(async _ =>
                    {
                        try
                        {
                            var activeState = this[nextState];
                            await activeState.OnStateBegan(token);
                            nextState = activeState.OnStateEnded();
                        }
                        catch (OperationCanceledException)
                        {
                            this.Red(
                                $"State Cancelled!! _lifeTimeCancellationToken cancelled? {_lifeTimeCancellationTokenSource.IsCancellationRequested} ?? {token.IsCancellationRequested}");
                        }
                    }, token);
            }
            catch (OperationCanceledException)
            {
                this.Red(
                    $"UniTaskAsyncEnumerable Cancelled!! _lifeTimeCancellationToken cancelled? {_lifeTimeCancellationTokenSource.IsCancellationRequested} ?? {token.IsCancellationRequested}");
            }
        }

        public void Dispose()
        {
            this.Cyan("Dispose");
            _lifeTimeCancellationTokenSource?.Cancel();
            _lifeTimeCancellationTokenSource?.Dispose();
        }
    }
}
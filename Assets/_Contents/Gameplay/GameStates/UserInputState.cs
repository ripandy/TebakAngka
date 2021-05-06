using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kassets.Utilities;
using MessagePipe;

namespace TebakAngka.Gameplay
{
    public class UserInputState : IGameState
    {
        private readonly GameModel _gameModel;
        private readonly IAsyncRequestHandler<GameStateEnum, int> _answerRequestHandler;

        public UserInputState(
            GameModel gameModel,
            IAsyncRequestHandler<GameStateEnum, int> answerRequestHandler)
        {
            _gameModel = gameModel;
            _answerRequestHandler = answerRequestHandler;
        }
        
        public async UniTask OnStateBegan(CancellationToken token)
        {
            this.Cyan("OnStateBegan");
            using var cts = new CancellationTokenSource();
            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, cts.Token);
            try
            {
                _gameModel.userAnswer = await _answerRequestHandler.InvokeAsync(GameStateEnum.UserInput, linkedTokenSource.Token);
                this.Orange($"Not Cancelled, token cancelled?: {token.IsCancellationRequested}, cts {(cts == null ? "is null" : $"cancelled? {cts.IsCancellationRequested}")}, linkedTokenSource cancelled?: {linkedTokenSource.IsCancellationRequested}");
            }
            catch (OperationCanceledException)
            {
                this.Orange($"Cancelled, token cancelled?: {token.IsCancellationRequested}, cts {(cts == null ? "is null" : $"cancelled? {cts.IsCancellationRequested}")}, linkedTokenSource cancelled?: {linkedTokenSource.IsCancellationRequested}");
            }
        }

        public GameStateEnum OnStateEnded()
        {
            this.Cyan("OnStateEnded");
            return GameStateEnum.CheckAnswer;
        }
    }
}
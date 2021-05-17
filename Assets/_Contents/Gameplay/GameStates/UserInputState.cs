using System.Threading;
using Cysharp.Threading.Tasks;
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
            using var cts = new CancellationTokenSource();
            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, cts.Token);
            _gameModel.userAnswer = await _answerRequestHandler.InvokeAsync(GameStateEnum.UserInput, linkedTokenSource.Token);
        }

        public GameStateEnum OnStateEnded()
        {
            return GameStateEnum.CheckAnswer;
        }
    }
}
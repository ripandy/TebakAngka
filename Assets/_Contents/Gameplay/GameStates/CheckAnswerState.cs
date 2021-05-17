using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace TebakAngka.Gameplay
{
    public class CheckAnswerState : IGameState
    {
        private readonly GameModel _gameModel;
        private readonly IAsyncPublisher<GameStateEnum, bool> _answerResultPublisher;
        
        private const GameStateEnum OwnState = GameStateEnum.CheckAnswer;

        public CheckAnswerState(
            GameModel gameModel,
            IAsyncPublisher<GameStateEnum, bool> answerResultPublisher)
        {
            _gameModel = gameModel;
            _answerResultPublisher = answerResultPublisher;
        }
        
        public async UniTask OnStateBegan(CancellationToken token)
        {
            await _answerResultPublisher.PublishAsync(OwnState, _gameModel.IsAnswerCorrect, token);
        }

        public GameStateEnum OnStateEnded()
        {
            return GameStateEnum.GenerateLevel;
        }
    }
}
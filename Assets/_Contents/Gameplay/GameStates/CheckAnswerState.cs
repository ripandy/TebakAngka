using System.Threading;
using Cysharp.Threading.Tasks;
using Kassets.Utilities;
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
            this.Cyan("OnStateBegan");
            await _answerResultPublisher.PublishAsync(OwnState, _gameModel.IsAnswerCorrect, token);
        }

        public GameStateEnum OnStateEnded()
        {
            this.Cyan("OnStateEnded");
            return GameStateEnum.GenerateLevel;
        }
    }
}
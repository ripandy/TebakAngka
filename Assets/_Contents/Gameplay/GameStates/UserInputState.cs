using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace TebakAngka.Gameplay
{
    public class UserInputState : IGameState
    {
        private readonly GameModel _gameModel;
        private readonly IAsyncPublisher<GameStateEnum> _requestInputPublisher;
        private readonly ISubscriber<GameStateEnum, int> _inputAnswerSubscriber;
        
        private const GameStateEnum OwnState = GameStateEnum.UserInput;
        
        private IDisposable _subscriptions;

        public UserInputState(
            GameModel gameModel,
            IAsyncPublisher<GameStateEnum> requestInputPublisher,
            ISubscriber<GameStateEnum, int> inputAnswerSubscriber)
        {
            _gameModel = gameModel;
            _requestInputPublisher = requestInputPublisher;
            _inputAnswerSubscriber = inputAnswerSubscriber;
        }
        
        public async UniTask OnStateBegan(CancellationToken token)
        {
            var bag = DisposableBag.CreateBuilder();
            _inputAnswerSubscriber.Subscribe(OwnState, i => _gameModel.userAnswer = i).AddTo(bag);
            _subscriptions = bag.Build();

            await _requestInputPublisher.PublishAsync(OwnState, token);
        }

        public GameStateEnum OnStateEnded()
        {
            _subscriptions.Dispose();
            return GameStateEnum.CheckAnswer;
        }
    }
}
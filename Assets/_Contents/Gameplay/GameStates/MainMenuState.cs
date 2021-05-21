using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;

namespace TebakAngka.Gameplay
{
    public class MainMenuState : IGameState
    {
        private readonly IAsyncPublisher<GameStateEnum> _gameStatePublisher;
        
        private const GameStateEnum OwnState = GameStateEnum.MainMenu;
        
        public MainMenuState(IAsyncPublisher<GameStateEnum>  gameStatePublisher)
        {
            _gameStatePublisher = gameStatePublisher;
        }
        
        public async UniTask OnStateBegan(CancellationToken token)
        {
            await _gameStatePublisher.PublishAsync(OwnState, token);
        }

        public GameStateEnum OnStateEnded()
        {
            return GameStateEnum.GenerateLevel;
        }
    }
}
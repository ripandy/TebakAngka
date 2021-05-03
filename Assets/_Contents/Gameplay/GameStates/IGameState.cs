using System.Threading;
using Cysharp.Threading.Tasks;

namespace TebakAngka.Gameplay
{
    public interface IGameState
    {
        UniTask OnStateBegan(CancellationToken token);
        GameStateEnum OnStateEnded();
    }
}
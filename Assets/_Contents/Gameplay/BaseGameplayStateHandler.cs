using Cysharp.Threading.Tasks;

namespace TebakAngka.Gameplay
{
    public interface IGameplayStateHandler
    {
        UniTask OnStateBegin();
        void OnStateEnded();
    }
}
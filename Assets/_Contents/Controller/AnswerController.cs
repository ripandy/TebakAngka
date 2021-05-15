using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using TebakAngka.Gameplay;
using TebakAngka.View;

namespace TebakAngka.Controller
{
    public class AnswerController : IAsyncRequestHandler<GameStateEnum, int>
    {
        private readonly IList<CardView> _cards;

        public AnswerController(IList<CardView> cards)
        {
            _cards = cards;
        }
        
        public async UniTask<int> InvokeAsync(GameStateEnum request, CancellationToken cancellationToken = default)
        {
            if (request != GameStateEnum.UserInput) return -1;
            
            var waitInputTask = new List<UniTask<int>>();
            foreach (var card in _cards)
            {
                if (card.Visible)
                    waitInputTask.Add(card.SelectAsync(cancellationToken));
            }

            var result = -1;
            
            if (waitInputTask.Count > 1)
            {
                (_, result) = await UniTask.WhenAny(waitInputTask);
            }
            else if (waitInputTask.Count == 1)
            {
                result = await waitInputTask[0];
            }
            
            return result;
        }
    }
}
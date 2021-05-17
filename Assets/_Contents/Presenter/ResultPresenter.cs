using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using TebakAngka.Gameplay;
using TebakAngka.View;
using VContainer.Unity;

namespace TebakAngka.Presenter
{
    public class ResultPresenter : IInitializable, IDisposable
    {
        private readonly IAsyncSubscriber<GameStateEnum, bool> _answerResultSubscriber;
        private readonly ResultView _correctResultView;
        private readonly ResultView _wrongResultView;
        
        private IDisposable _subscription;
        
        public ResultPresenter(
            IAsyncSubscriber<GameStateEnum, bool> answerResultSubscriber,
            ResultView[] resultViews)
        {
            _answerResultSubscriber = answerResultSubscriber;
            _correctResultView = resultViews[0];
            _wrongResultView = resultViews[1];
        }
        
        public void Initialize()
        {
            var bag = DisposableBag.CreateBuilder();

            _answerResultSubscriber.Subscribe(GameStateEnum.CheckAnswer, HandleResult).AddTo(bag);

            _subscription = bag.Build();
        }

        private async UniTask HandleResult(bool result, CancellationToken token)
        {
            if (result)
                await _correctResultView.Show(token);
            else
                await _wrongResultView.Show(token);
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}
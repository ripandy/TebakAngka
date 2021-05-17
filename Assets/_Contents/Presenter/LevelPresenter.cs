using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using TebakAngka.Gameplay;
using TebakAngka.View;
using UnityEngine;
using VContainer.Unity;

namespace TebakAngka.Presenter
{
    public class LevelPresenter : IInitializable, IDisposable
    {
        private readonly IAsyncSubscriber<GameStateEnum, IList<int>> _answersSubscriber;
        private readonly Func<CardView> _cardFactory;
        private readonly IList<CardView> _cards;

        private IDisposable _subscription;
        
        public LevelPresenter(
            IAsyncSubscriber<GameStateEnum, IList<int>> answersSubscriber,
            Func<CardView> cardFactory,
            IList<CardView> cards)
        {
            _answersSubscriber = answersSubscriber;
            _cardFactory = cardFactory;
            _cards = cards;
        }
        
        public void Initialize()
        {
            var bag = DisposableBag.CreateBuilder();
            
            _answersSubscriber.Subscribe(GameStateEnum.GenerateLevel, SetupLevel).AddTo(bag);
            
            _subscription = bag.Build();
        }

        private async UniTask SetupLevel(IList<int> answers, CancellationToken token)
        {
            var count = Mathf.Max(answers.Count, _cards.Count);
            
            var tasks = new List<UniTask>();
            for (var i = 0; i < count; i++)
            {
                if (_cards.Count <= i)
                    _cards.Add(_cardFactory.Invoke());
                
                var card = _cards[i];
                    card.Visible = i < answers.Count;
                    
                tasks.Add(card.InitView(answers[i], token));
            }

            await UniTask.WhenAll(tasks);
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}
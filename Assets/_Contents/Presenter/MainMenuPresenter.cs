using System;
using Cysharp.Threading.Tasks;
using MessagePipe;
using TebakAngka.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace TebakAngka.Presenter
{
    public class MainMenuPresenter : IInitializable, IDisposable
    {
        private readonly IAsyncSubscriber<GameStateEnum> _gameStateSubscriber;
        private readonly GameObject _baseObject;
        private readonly Button _startButton;

        private IDisposable _subscription;

        public MainMenuPresenter(
            IAsyncSubscriber<GameStateEnum> gameStateSubscriber,
            GameObject baseObject,
            Button startButton)
        {
            _gameStateSubscriber = gameStateSubscriber;
            _baseObject = baseObject;
            _startButton = startButton;
        }
        
        public void Initialize()
        {
            var bag = DisposableBag.CreateBuilder();
            
            _gameStateSubscriber.Subscribe(async (stateEnum, token) =>
            {
                if (stateEnum != GameStateEnum.MainMenu)
                    return;

                _baseObject.SetActive(true);
                await _startButton.OnClickAsync(token);
                _baseObject.SetActive(false);
            }).AddTo(bag);
            
            _subscription = bag.Build();
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}
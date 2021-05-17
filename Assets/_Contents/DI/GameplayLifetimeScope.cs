using System.Collections.Generic;
using TebakAngka.Controller;
using TebakAngka.Gameplay;
using TebakAngka.Presenter;
using TebakAngka.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using MessagePipe;

namespace TebakAngka.DI
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameObject _cardViewPrefab;
        [SerializeField] private Transform _cardViewParent;
        [SerializeField] private ResultView[] _resultViews;
        [SerializeField] private AudioClipCollection[] _levelIntroClipCollections;
        [SerializeField] private AudioClipCollection[] _resultClipCollections;
        [SerializeField] private AudioSource _audioSource;
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterDomain(builder);
            RegisterMessagePipe(builder);
            RegisterPresenter(builder);
            RegisterController(builder);
            RegisterView(builder);
        }

        private void RegisterDomain(IContainerBuilder builder)
        {
            // Register Model.
            builder.Register<GameModel>(Lifetime.Scoped);
            
            // Register GameStateEngine as entry point.
            builder.RegisterEntryPoint<GameStateEngine>(Lifetime.Scoped);
            
            // Register GameStates by interface IGameState.
            builder.Register<GenerateLevelState>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<UserInputState>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<CheckAnswerState>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void RegisterMessagePipe(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();
            
            // used by GenerateLevelState -> LevelPresenter
            builder.RegisterMessageBroker<GameStateEnum, IList<int>>(options);
            builder.RegisterMessageBroker<GameStateEnum, int>(options);
            
            // used by CheckAnswerState -> ?
            builder.RegisterMessageBroker<GameStateEnum, bool>(options);
        }

        private void RegisterPresenter(IContainerBuilder builder)
        {
            builder.Register<LevelPresenter>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<LevelAudioPresenter>(Lifetime.Scoped).AsImplementedInterfaces().WithParameter("audioClipCollections", _levelIntroClipCollections);
            builder.Register<ResultPresenter>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<ResultAudioPresenter>(Lifetime.Scoped).AsImplementedInterfaces().WithParameter("audioClipCollections", _resultClipCollections);
        }
        
        private void RegisterController(IContainerBuilder builder)
        {
            // interface IAsyncRequestHandler<GameStateEnum, int> is referenced by Domain class UserInputState.
            builder.Register<AnswerController>(Lifetime.Scoped).AsImplementedInterfaces();
        }
        
        private void RegisterView(IContainerBuilder builder)
        {
            // audio
            builder.RegisterInstance(_audioSource);
            
            // IList<CardView> is shared and referenced by LevelPresenter and AnswerController.
            builder.RegisterInstance(new List<CardView>()).AsImplementedInterfaces();
            builder.RegisterInstance(_resultViews);
            
            // register CardView factory. Factory is referenced by LevelPresenter.
            builder.RegisterFactory<CardView>(container =>
                {
                    CardView InstantiateCardView()
                    {
                        return container.Instantiate(_cardViewPrefab, _cardViewParent).GetComponent<CardView>();
                    }

                    return InstantiateCardView;
                },
                Lifetime.Scoped);
        }
    }
}
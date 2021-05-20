using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using TebakAngka.Gameplay;
using TebakAngka.View;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace TebakAngka.Presenter
{
    public class ResultAudioPresenter : IInitializable, IDisposable
    {
        private readonly IAsyncSubscriber<GameStateEnum, bool> _answerResultSubscriber;
        private readonly AudioClipCollection _resultCorrectClipsIbun;
        private readonly AudioClipCollection _resultCorrectClipsRanca;
        private readonly AudioClipCollection _resultWrongClipsIbun;
        private readonly AudioSource _audioSource;
        
        private IDisposable _subscription;

        public ResultAudioPresenter(
            IAsyncSubscriber<GameStateEnum, bool> answerResultSubscriber,
            AudioClipCollection[] audioClipCollections,
            AudioSource audioSource)
        {
            _answerResultSubscriber = answerResultSubscriber;
            _resultCorrectClipsIbun = audioClipCollections[0];
            _resultCorrectClipsRanca = audioClipCollections[1];
            _resultWrongClipsIbun = audioClipCollections[2];
             _audioSource = audioSource;
        }
        
        public void Initialize()
        {
            var bag = DisposableBag.CreateBuilder();
            
            _answerResultSubscriber.Subscribe(GameStateEnum.CheckAnswer, PlayResultAudio).AddTo(bag);
            
            _subscription = bag.Build();
        }

        private async UniTask PlayResultAudio(bool isCorrect, CancellationToken token)
        {
            if (isCorrect)
            {
                var baseIdx = Random.Range(0, 2);
                _audioSource.clip = baseIdx == 0
                    ? _resultCorrectClipsIbun[Random.Range(0, _resultCorrectClipsIbun.Count)]
                    : _resultCorrectClipsRanca[Random.Range(0, _resultCorrectClipsRanca.Count)];
            }
            else
            {
                _audioSource.clip = _resultWrongClipsIbun[Random.Range(0, _resultWrongClipsIbun.Count)];
            }
            
            _audioSource.pitch = Random.Range(0.95f, 1.05f);
            _audioSource.Play();

            await UniTask.WaitUntil(() => !_audioSource.isPlaying && _audioSource.time >= _audioSource.clip.length,
                cancellationToken: token);
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}
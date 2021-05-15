using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Feature.AudioManagement
{
    public class UnityDelayedAudioHandler : BaseAudioHandler
    {
        [Header("Unity")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _delay;
        
        protected override void PlayAudio(int index)
        {
            _audioSource.clip = _audioClipCollection[index];
            if (_onPlayAudioCompleted != null)
            {
                PlayAndRaiseCompleted().Forget();
            }
            else
            {
                _audioSource.PlayDelayed(_delay);
            }
        }

        private async UniTask PlayAndRaiseCompleted()
        {
            _audioSource.PlayDelayed(_delay);
            await UniTask.WaitUntil(() => !_audioSource.isPlaying && _audioSource.time >= _audioSource.clip.length, cancellationToken: this.GetCancellationTokenOnDestroy());
            _onPlayAudioCompleted.Raise();
        }
    }
}
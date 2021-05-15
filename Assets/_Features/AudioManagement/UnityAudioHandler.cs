using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Feature.AudioManagement
{
    public class UnityAudioHandler : BaseAudioHandler
    {
        [Header("Unity")]
        [SerializeField] private AudioSource _audioSource;
        
        protected override void PlayAudio(int index)
        {
            _audioSource.clip = _audioClipCollection[index];
            
            if (_onPlayAudioCompleted != null)
            {
                PlayAndRaiseCompleted().Forget();
            }
            else
            {
                _audioSource.Play();
            }
        }

        private async UniTask PlayAndRaiseCompleted()
        {
            _audioSource.Play();
            await UniTask.WaitUntil(() => !_audioSource.isPlaying && _audioSource.time >= _audioSource.clip.length, cancellationToken: this.GetCancellationTokenOnDestroy());
            _onPlayAudioCompleted.Raise();
        }
    }
}
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Kassets.EventSystem;
using UnityEngine;

namespace Feature.AudioManagement
{
    public abstract class BaseAudioHandler : MonoBehaviour
    {
        [Header("Audio Clip Collection Reference")]
        [SerializeField] protected AudioClipCollection _audioClipCollection;
        
        [Header("Input Event")]
        [SerializeField] private IntEvent _playAudioEvent;
        
        [Header("Output Event")]
        [SerializeField] protected GameEvent _onPlayAudioCompleted;

        private void Start()
        {
            if (_playAudioEvent != null)
            {
                _playAudioEvent.Subscribe(PlayAudio, this.GetCancellationTokenOnDestroy());
            }
            else
            {
                var idx = Random.Range(0, _audioClipCollection.Count);
                PlayAudio(idx);
            }
        }

        protected abstract void PlayAudio(int index);
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace TebakAngka.View
{
    [CreateAssetMenu(fileName = "AudioClipCollection", menuName = "ScriptableObjects/AudioClipCollection")]
    public class AudioClipCollection : ScriptableObject
    {
        [SerializeField] private List<AudioClip> _audioClips;
        public AudioClip this[int index] => _audioClips[index];
        public int Count => _audioClips.Count;
    }
}
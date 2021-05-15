using Kassets;
using Kassets.Collection;
using UnityEngine;

namespace Feature.AudioManagement
{
    [CreateAssetMenu(fileName = "AudioClipCollection", menuName = MenuHelper.DefaultCollectionMenu + "AudioClipCollection")]
    public class AudioClipCollection : Collection<AudioClip>
    {
    }
}
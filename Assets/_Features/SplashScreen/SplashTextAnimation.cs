using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Feature.SplashScreen
{
    public class SplashTextAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform[] _charTransforms;

        private static readonly Vector2 MinMaxDelta = new Vector2(-5f, 5f);
        private const float RotationMultiplier = 1.6f;

        private void Start()
        {
            UniTaskAsyncEnumerable.Interval(TimeSpan.FromMilliseconds(250))
                .Subscribe(_ => RandomizePosition(), this.GetCancellationTokenOnDestroy());
        }

        private void RandomizePosition()
        {
            foreach (var charTransform in _charTransforms)
            {
                var pos = charTransform.anchoredPosition;
                    pos.x = Random.Range(MinMaxDelta.x, MinMaxDelta.y);
                    pos.y = Random.Range(MinMaxDelta.x, MinMaxDelta.y);

                var rot = charTransform.eulerAngles;
                    rot.z = Random.Range(MinMaxDelta.x, MinMaxDelta.y) * RotationMultiplier;

                charTransform.anchoredPosition = pos;
                charTransform.eulerAngles = rot;
            }
        }
    }
}
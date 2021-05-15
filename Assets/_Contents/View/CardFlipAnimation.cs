using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace TebakAngka.View
{
    public class CardFlipAnimation : MonoBehaviour
    {
        private enum FlipDirection
        {
            Horizontal,
            Vertical
        }

        [SerializeField] private Transform _baseTransform;
        [SerializeField] private GameObject _content;
        [SerializeField] private FlipDirection _flipDirection = FlipDirection.Vertical;
        [Min(0f)] [SerializeField] private float AnimationDuration = 0.3f;

        private Sequence GenerateFlipSequence(bool flipOpen)
        {
            var scaleAnimationOut = _flipDirection == FlipDirection.Horizontal ? _baseTransform.DOScaleY(0f, AnimationDuration).From(1f) : _baseTransform.DOScaleX(0f, AnimationDuration).From(1f);
            var scaleAnimationIn = _flipDirection == FlipDirection.Horizontal ? _baseTransform.DOScaleY(1f, AnimationDuration).From(0f) : _baseTransform.DOScaleX(1f, AnimationDuration).From(0f);
            
            _content.SetActive(!flipOpen);
            return DOTween.Sequence()
                .Append(scaleAnimationOut)
                .AppendCallback(() => _content.SetActive(flipOpen))
                .Append(scaleAnimationIn);
        }

        public void AnimateFlip(bool flipOpen)
        {
            GenerateFlipSequence(flipOpen);
        }

        public UniTask AnimateFlipAsync(bool flipOpen, CancellationToken token = default)
        {
            return GenerateFlipSequence(flipOpen).ToUniTask(cancellationToken: token == default ? this.GetCancellationTokenOnDestroy() : token);
        }

        public void ToggleFlip()
        {
            var toOpen = !_content.activeInHierarchy;
            AnimateFlip(toOpen);
        }
        
        public UniTask ToggleFlipAsync(CancellationToken token = default)
        {
            var toOpen = !_content.activeInHierarchy;
            return AnimateFlipAsync(toOpen, token);
        }
    }
}
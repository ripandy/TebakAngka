using DG.Tweening;
using UnityEngine;

namespace Feature.MainMenu
{
    public class MainMenuTextAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _labelTransform;

        private Sequence _animation;
        
        private const float ScaleValueX = 1.04f;
        private const float ScaleValueY = 1.16f;
        private const float IdleDuration = 0.5f;

        private void Start()
        {
            _animation = DOTween.Sequence().Append(_labelTransform.DOScaleX(ScaleValueX, IdleDuration).SetEase(Ease.Linear))
                .Join(_labelTransform.DOScaleY(1, IdleDuration).From(ScaleValueY).SetEase(Ease.Linear))
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDestroy()
        {
            _animation.Kill();
        }
    }
}
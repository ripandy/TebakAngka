using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using DG.Tweening;
using Kassets.VariableSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.LoadingScreen
{
    public class CanvasImageFadeHandler : BaseFadeHandler
    {
        [SerializeField] private ColorVariable _fadeColor;
        [SerializeField] private Image _image;

        protected override void Initialize(CancellationToken token)
        {
            base.Initialize(token);
            _fadeColor.Subscribe(SetFadeColor, token);
        }
        
        private void SetFadeColor(Color color)
        {
            _image.color = color;
        }

        protected override async UniTask StartFade(float duration, bool fadeIn)
        {
            var token = this.GetCancellationTokenOnDestroy();
            var fadeTo = fadeIn ? 0f : 1f;
            var fadeFrom = fadeIn ? 1f : 0f;
            
            if (!fadeIn)
                _image.gameObject.SetActive(true);
            
            await DOTween.ToAlpha(() => _image.color, value => _image.color = value, fadeTo, duration).From(fadeFrom)
                .ToUniTask(cancellationToken: token);
            
            if (fadeIn)
                _image.gameObject.SetActive(false);
        }
    }
}
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TebakAngka.View
{
    public class ResultView : MonoBehaviour
    {
        [SerializeField] private Transform _baseTransform;
        [SerializeField] private Image _overlay;

        public async UniTask Show(CancellationToken token)
        {
            _overlay.gameObject.SetActive(true);
            
            var targetAlpha = 0.4f;
            var fadeDuration = 0.3f;
            DOTween.ToAlpha(() => _overlay.color, color => _overlay.color = color, targetAlpha, fadeDuration).From(0f);
            
            var duration = 0.05f;
            
            await _baseTransform.DOScaleX(1f, duration).From(0f).SetEase(Ease.Linear).ToUniTask(cancellationToken: token);

            while (duration < 1f)
            {
                await _baseTransform.DOScaleX(-1f, duration).From(1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo).ToUniTask(cancellationToken: token);
                duration = Mathf.Min(1f, duration * 2);
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: token);
            
            DOTween.ToAlpha(() => _overlay.color, color => _overlay.color = color, 0, fadeDuration).From(targetAlpha);
            
            await _baseTransform.DOScaleX(0f, fadeDuration).From(1f).ToUniTask(cancellationToken: token);
            
            _overlay.gameObject.SetActive(false);
        }
    }
}
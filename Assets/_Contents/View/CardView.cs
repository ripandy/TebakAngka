using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TebakAngka.View
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private CardFlipAnimation _cardFlipAnimation;
        [SerializeField] private Button _button;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _numberImage;
        [SerializeField] private Sprite[] _cardSprites;

        [Header("Number Color")]
        [SerializeField] private Color _brightColor = new Color(0.9294118f, 0.9294118f, 0.9294118f);
        [SerializeField] private Color _darkColor = new Color(0.07058824f, 0.07058824f, 0.07058824f);

        private int _number;
        
        public bool Visible
        {
            get => gameObject.activeInHierarchy;
            set => gameObject.SetActive(value);
        }

        public async UniTask InitView(int number, CancellationToken cancellationToken = default)
        {
            _number = number;
            _numberImage.sprite = _cardSprites[number];
            _backgroundImage.color = RandomColor(out var isLight);
            _numberImage.color = isLight ? _darkColor : _brightColor;

            _button.interactable = false;
            await _cardFlipAnimation.AnimateFlipAsync(true, cancellationToken);
            _button.interactable = true;
        }

        public async UniTask<int> SelectAsync(CancellationToken token)
        {
            await _button.OnClickAsync(token);
            return _number;
        }

        private Color RandomColor(out bool isLight)
        {
            var r = Random.Range(0, 1f);
            var g = Random.Range(0, 1f);
            var b = Random.Range(0, 1f);

            var rb = r * 255;
            var gb = g * 255;
            var bb = b * 255;

            var brightness = Mathf.Sqrt(
                0.299f * (rb * rb) +
                0.587f * (gb * gb) +
                0.114f * (bb * bb)
            );

            isLight = brightness > 160;

            return new Color(r, g, b);
        }
    }
}
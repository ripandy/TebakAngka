using TMPro;
using UnityEngine;

namespace Feature.LoadingScreen
{
    public class ProgressBarHandler : BaseLoadingProgressHandler
    {
        [SerializeField] private Transform _progressionBar;
        [SerializeField] private TMP_Text _progressText;
        
        protected override void UpdateProgression(float progress)
        {
            var s = _progressionBar.localScale;
                s.x = progress;
            _progressionBar.localScale = s;

            var percent = Mathf.FloorToInt(progress * 100);
            _progressText.text = $"Now Loading... ({percent}%)";
        }
    }
}
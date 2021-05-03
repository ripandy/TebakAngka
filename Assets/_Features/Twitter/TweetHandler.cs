using Cysharp.Threading.Tasks;
using Kassets.EventSystem;
using UnityEngine;
using UnityEngine.Networking;

namespace Feature.Twitter
{
    public class TweetHandler : MonoBehaviour
    {
        [SerializeField] private string _tweetText;
        [SerializeField] private string _tweetUrl;
        [SerializeField] private string[] _tweetHashtags;
        [SerializeField] private GameEvent _tweetTrigger;

        private const string BaseUrl = "https://twitter.com/intent/tweet?=";

        private void Start()
        {
            var token = this.GetCancellationTokenOnDestroy();
            _tweetTrigger.Subscribe(PostTweet, token);
        }

        private void PostTweet()
        {
            var textParam = string.IsNullOrEmpty(_tweetText) ? "" : $"&text={UnityWebRequest.EscapeURL(_tweetText)}";
            var urlParam = string.IsNullOrEmpty(_tweetUrl) ? "" : $"&url={UnityWebRequest.EscapeURL(_tweetUrl)}";
            var hashtagString = "";
            for (var i = 0; i < _tweetHashtags.Length; i++)
            {
                hashtagString += $"{(i == 0 ? "" : ",")}{_tweetHashtags[i]}";
            }

            var hashtagParam = string.IsNullOrEmpty(hashtagString)
                ? ""
                : $"&hashtags={UnityWebRequest.EscapeURL(hashtagString)}";
            var url = $"{BaseUrl}{urlParam}{textParam}{hashtagParam}";
            Debug.Log($"{BaseUrl}{urlParam}{textParam}{hashtagParam}");
            Debug.Log($"{url}");
            Application.OpenURL(url);
        }
    }
}
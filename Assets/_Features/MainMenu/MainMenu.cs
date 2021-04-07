using Cysharp.Threading.Tasks;
using Kassets.EventSystem;
using Feature.ApplicationStateManagement;
using UnityEngine;

namespace Feature.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private ApplicationStateVariable applicationState;
        [SerializeField] private GameEvent gameStartEvent;

        private void Start()
        {
            gameStartEvent
                .Subscribe(() => applicationState.Value = ApplicationStateEnum.GamePlay)
                .AddTo(this.GetCancellationTokenOnDestroy());
        }
    }
}
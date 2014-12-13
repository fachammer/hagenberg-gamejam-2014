using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;

namespace TheDarkNight.GameManager {

    [RequireComponent(typeof(IGameManager))]
    public class GamePauser : MonoBehaviour {
        private IGameManager gameManager;

        private CompositeDisposable gameManagerSubscriptions = new CompositeDisposable();

        private void Start() {
            gameManager = this.TryGetClass<IGameManager>();
            gameManager.GameEnded.Subscribe(ended => UnityEngine.Time.timeScale = ended ? 0 : 1).AddTo(gameManagerSubscriptions);
            gameManager.GamePaused.Subscribe(paused => UnityEngine.Time.timeScale = paused ? 0 : 1).AddTo(gameManagerSubscriptions);
        }

        private void OnDisable() {
            gameManagerSubscriptions.Clear();
        }
    }
}
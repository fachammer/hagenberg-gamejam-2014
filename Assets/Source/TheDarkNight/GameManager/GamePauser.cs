using System;
using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;

namespace TheDarkNight.GameManager {

    [RequireComponent(typeof(IGameManager))]
    public class GamePauser : MonoBehaviour {
        private IGameManager gameManager;

        private IDisposable gameManagerSubscription = Disposable.Empty;

        private void Start() {
            gameManager = this.TryGetClass<IGameManager>();
            gameManagerSubscription = gameManager.GamePaused.Subscribe(paused => UnityEngine.Time.timeScale = paused ? 0 : 1);
        }

        private void OnDisable() {
            gameManagerSubscription.Dispose();
        }
    }
}
using PRO3.Utility;
using System;
using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;

namespace TheDarkNight.GameManager {

    [RequireComponent(typeof(IGameManager))]
    public class GameQuitter : MonoBehaviour {
        private IDisposable gameManagerSubscription = Disposable.Empty;

        private void Start() {
            IGameManager gameManager = this.TryGetClass<IGameManager>();
            gameManagerSubscription = gameManager.GameQuit
                .DebugLog("quit")
                .Subscribe(_ => Application.Quit());
        }

        private void OnDisable() {
            gameManagerSubscription.Dispose();
        }
    }
}
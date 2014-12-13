using System;
using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TheDarkNight.GameManager {

    [RequireComponent(typeof(Text))]
    public class ControlsUI : MonoBehaviour {
        private IDisposable gameManagerSubscription = Disposable.Empty;

        [SerializeField]
        private GameManager gameManager;

        private void Start() {
            Text text = this.TryGetComponent<Text>();
            gameManagerSubscription = gameManager.GameStarted.Subscribe(started => text.enabled = !started);
            text.enabled = !gameManager.GameStarted;
        }

        private void OnDisable() {
            gameManagerSubscription.Dispose();
        }
    }
}
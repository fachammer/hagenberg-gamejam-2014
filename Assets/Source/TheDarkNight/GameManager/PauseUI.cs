using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TheDarkNight.GameManager {

    [RequireComponent(typeof(Text))]
    public class PauseUI : MonoBehaviour {

        [SerializeField]
        private GameManager gameManager;

        private Text text;

        private void Start() {
            text = this.TryGetComponent<Text>();
            gameManager.GamePaused.Subscribe(paused => text.enabled = paused);
            text.enabled = gameManager.GamePaused;
        }
    }
}
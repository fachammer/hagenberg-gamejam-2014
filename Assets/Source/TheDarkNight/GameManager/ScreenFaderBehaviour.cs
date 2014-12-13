using ModestTree.Zenject;
using TheDarkNight.Extensions;
using TheDarkNight.Observables.Time;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TheDarkNight.GameManager {

    [RequireComponent(typeof(Image))]
    public class ScreenFaderBehaviour : MonoBehaviour {
        private ImageFader fader;

        [SerializeField]
        private GameManager gameManager;

        [SerializeField]
        private float fadeResolution;

        [SerializeField]
        private Color fadeToColorAtBeginning;

        [SerializeField]
        private AnimationCurve colorFadeAnimation;

        [Inject]
        public IObservableTime Time { get; set; }

        private void Start() {
            Image image = this.TryGetComponent<Image>();
            fader = new ImageFader(image, fadeResolution);

            gameManager.GameStarted.Where(s => !s).Subscribe(_ => fader.FadeToColor(Time, fadeToColorAtBeginning, colorFadeAnimation));
        }
    }
}
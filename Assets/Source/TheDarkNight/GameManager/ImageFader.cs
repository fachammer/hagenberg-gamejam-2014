using System.Linq;
using TheDarkNight.Observables.Time;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TheDarkNight.GameManager {

    public class ImageFader {
        private Image image;
        private float fadeResolution;

        public ImageFader(Image image, float fadeResolution) {
            this.image = image;
            this.fadeResolution = fadeResolution;
        }

        public void FadeToColor(IObservableTime time, Color color, AnimationCurve fadeAnimation) {
            Color currentColor = image.color;
            float fadeDuration = fadeAnimation.keys.Last().time;
            float fadeUpdateInterval = 1 / fadeResolution;
            time.ElapsedIntervals(fadeUpdateInterval)
                .Select(tick => tick * fadeUpdateInterval)
                .TakeWhile(t => t <= fadeDuration)
                .Subscribe(t => image.color = Color.Lerp(currentColor, color, fadeAnimation.Evaluate(t)), () => image.color = color);
        }
    }
}
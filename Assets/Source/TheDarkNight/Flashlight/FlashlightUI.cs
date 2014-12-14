using TheDarkNight.Darkness;
using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TheDarkNight.FlashLight {

    [RequireComponent(typeof(Image))]
    public class FlashlightUI : MonoBehaviour {

        [SerializeField]
        private Killable killable;

        [SerializeField]
        private FlashLight flashLight;

        [SerializeField]
        private float totalEnergy;

        private Image image;

        private void Start() {
            image = this.TryGetComponent<Image>();
            killable.Killed.Subscribe(_ => image.enabled = false);
        }

        private void Update() {
            image.fillAmount = flashLight.GetBatteryLoad() / totalEnergy;
        }
    }
}
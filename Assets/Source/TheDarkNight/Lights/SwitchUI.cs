using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TheDarkNight.Lights {

    [RequireComponent(typeof(Image))]
    public class SwitchUI : MonoBehaviour {
        private Image image;

        [SerializeField]
        private Switcher switcher;

        private ISwitch switchToShow;

        private void Awake() {
            image = this.TryGetComponent<Image>();
            switchToShow = this.TryGetClassInParent<ISwitch>();
        }

        private void Start() {
            switcher.ToggleableSwitch.Subscribe(HandleToggleableSwitch);
        }

        private void HandleToggleableSwitch(ISwitch toggleSwitch) {
            image.enabled = toggleSwitch != null && toggleSwitch == switchToShow;
        }
    }
}
using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TheDarkNight.Picking {

    [RequireComponent(typeof(Image))]
    public class PickableUI : MonoBehaviour {
        private Image image;

        [SerializeField]
        private Picker picker;

        private IPickable pickable;

        private void Awake() {
            image = this.TryGetComponent<Image>();
            pickable = this.TryGetClassInParent<IPickable>();
        }

        private void Start() {
            picker.CanPickup.Subscribe(HandleCanPickup);
        }

        private void HandleCanPickup(IPickable pickable) {
            image.enabled = pickable != null && pickable == this.pickable;
        }
    }
}
using System;
using TheDarkNight.Extensions;
using TheDarkNight.Lights;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TheDarkNight.Picking {

    [RequireComponent(typeof(Image))]
    public class PickableUI : MonoBehaviour {
        private Image image;

        IDisposable subscription = Disposable.Empty;

        [SerializeField]
        private Picker picker;

        private IPickable pickable;

        private void Awake() {
            image = this.TryGetComponent<Image>();
            pickable = this.TryGetClassInParent<IPickable>();
        }

        private void Start() {
           subscription = picker.CanPickup.Subscribe(HandleCanPickup);
        }

        private void OnDestroy() {
            subscription.Dispose();
        }

        private void HandleCanPickup(IPickable pickable) {
            if(!gameObject.activeInHierarchy)
                return;
            Pickable p = this.pickable as Pickable;

            if(pickable == null)
                image.enabled = false;
            /*else if(p != null && p.GetComponent<LightBulbStack>() != null) {
                image.enabled = true;
            }*/
            else {
                image.enabled = pickable != null && pickable == this.pickable;
            }
        }
    }
}
using ModestTree.Zenject;
using TheDarkNight.Extensions;
using TheDarkNight.FlashLight;
using TheDarkNight.Observables.Time;
using TheDarkNight.Utility;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Picking {

    [RequireComponent(typeof(Inventory))]
    public class Picker : MonoBehaviour, IPicker {
        private ObservableProperty<IPickable> pickable = new ObservableProperty<IPickable>(null);
        private IInventory inventory;

        private ISubject<IPickable> picking = new Subject<IPickable>();

        [Inject]
        public GameObjectInstantiator GOI { get; set; }

        [Inject]
        public IObservableTime Time { get; set; }

        public IObservable<IPickable> Picking {
            get { return picking; }
        }

        public IObservable<IPickable> CanPickup {
            get { return pickable; }
        }

        public void CanPickupPickable(IPickable pickable) {
            this.pickable.Value = pickable;
        }

        public void CannotPickupPickable() {
            this.pickable.Value = null;
        }

        public void TryPickUpPickable() {
            if(this.pickable.Value != null && inventory.AddItem(pickable.Value) && pickable.Value.CanBePickedUpBy(this)) {
                if(pickable.Value.GetTransform().gameObject.name.Contains("SPECIAL")) {
                    GameObject clone = GOI.Instantiate(pickable.Value.GetTransform().gameObject);
                    pickable.Value.GetTransform().gameObject.name = "LightBulb";
                    clone.transform.position = pickable.Value.GetTransform().position;
                }

                if(pickable.Value.GetTransform().gameObject.name == "battery") {
                    Transform parent = pickable.Value.GetTransform().parent;
                    if(parent.GetComponent<UnityEngine.Animation>() != null)
                        parent.GetComponent<UnityEngine.Animation>().Play();
                }

                pickable.Value.GetTransform().parent = this.transform;
                pickable.Value.GetTransform().position = new Vector3(0, -50, 0);
                picking.OnNext(pickable.Value);
                pickable.Value.CannotBePickedupByOthersThan(this);
                this.pickable.Value = null;
            }
        }

        private void Start() {
            inventory = this.TryGetClass<IInventory>();
        }
    }
}
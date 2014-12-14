using TheDarkNight.Extensions;
using TheDarkNight.Utility;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Picking {

    [RequireComponent(typeof(Inventory))]
    public class Picker : MonoBehaviour, IPicker {
        private ObservableProperty<IPickable> pickable = new ObservableProperty<IPickable>(null);
        private IInventory inventory;

        private ISubject<IPickable> picking = new Subject<IPickable>();

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
                pickable.Value.GetTransform().parent = this.transform;
                pickable.Value.GetTransform().position = new Vector3(0, 0, -20);
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
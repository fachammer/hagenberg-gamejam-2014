using TheDarkNight.Extensions;
using UniRx;
using UnityEngine;

namespace TheDarkNight.Picking {

    [RequireComponent(typeof(Inventory))]
    public class Picker : MonoBehaviour, IPicker {
        private IPickable pickable;
        private IInventory inventory;

        private ISubject<IPickable> picking = new Subject<IPickable>();

        public IObservable<IPickable> Picking {
            get { return picking; }
        }

        public void CanPickupPickable(IPickable pickable) {
            this.pickable = pickable;
            PickUpPickable();//TODO REMOVE
        }

        public void CannotPickupPickable(IPickable pickable) {
            this.pickable = null;
        }

        public void PickUpPickable() {
            if(this.pickable != null && inventory.AddItem(pickable)) {
                pickable.GetTransform().parent = this.transform;
                pickable.GetTransform().position = new Vector3(0, 0, -20);
                picking.OnNext(pickable);
                this.pickable = null;
            }
        }

        private void Start() {
            inventory = this.TryGetClass<IInventory>();
        }
    }
}
using TheDarkNight.Extensions;
using UnityEngine;

namespace TheDarkNight.Picking {

    [RequireComponent(typeof(Inventory))]
    public class Picker : MonoBehaviour, IPicker {
        private IPickable pickable;
        private IInventory inventory;

        public void CanPickupPickable(IPickable pickable) {
            this.pickable = pickable;
            PickUpPickable();
        }

        public void CannotPickupPickable(IPickable pickable) {
            this.pickable = null;
        }

        public void PickUpPickable() {
            if(this.pickable != null && inventory.AddItem(pickable)) {
                pickable.GetTransform().parent = this.transform;
                pickable.GetTransform().position = new Vector3(0, 0, -20);
            }
        }

        private void Start() {
            inventory = this.TryGetClass<IInventory>();
        }
    }
}
using UnityEngine;
using System.Collections;
using TheDarkNight.Extensions;

namespace TheDarkNight.Picking {

    [RequireComponent(typeof(Inventory))]
    public class Picker : MonoBehaviour, IPicker {

        private bool canPickUp = false;
        private IPickable pickable;
        private IInventory inventory;

        public void CanPickupPickable(IPickable pickable) {
            canPickUp = true;
            this.pickable = pickable;
        }

        public void CannotPickupPickable(IPickable pickable) {
            canPickUp = false;
            this.pickable = pickable;
        }

        public void PickUpPickable() {
            if(canPickUp && this.pickable != null && inventory.AddItem(pickable)) {
                pickable.GetTransform().parent = this.transform;
                pickable.GetTransform().position = new Vector3(0, 0, -20);
            }
        }

        private void Start() {
            inventory = this.TryGetClass<IInventory>();
        }
    }

}
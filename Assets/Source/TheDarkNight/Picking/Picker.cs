using UnityEngine;
using System.Collections;
using TheDarkNight.Extensions;

namespace TheDarkNight.Picking {

    [RequireComponent(typeof(Inventory))]
    public class Picker : MonoBehaviour, IPicker {

        private bool canPickUp = false;        
        private GameObject pickable;
        private IInventory inventory;

        public void CanPickupPickable(GameObject pickable) {
            canPickUp = true;
            this.pickable = pickable;
            PickUpPickable();
        }

        public void CannotPickupPickable(GameObject pickable) {
            canPickUp = false;
            this.pickable = pickable;
        }

        public void PickUpPickable() {
            if(canPickUp && this.pickable != null && inventory.AddItem(pickable)) {
                pickable.transform.parent = this.transform;
                pickable.transform.position = new Vector3(0, 0, -20);
            }
        }

        private void Start() {
            inventory = this.TryGetClass<IInventory>();
        }
    }

}
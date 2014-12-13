using UnityEngine;
using System.Collections;
using TheDarkNight.Extensions;

namespace TheDarkNight.Picking {

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
            if(canPickUp && inventory.AddPickable(pickable))
                pickable.PickUp();
        }

        private void Start() {
            inventory = this.TryGetClass<IInventory>();
        }
    }

}
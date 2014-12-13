using UnityEngine;
using System.Collections;

namespace TheDarkNight.Picking {

    public class Picker : MonoBehaviour, IPicker {

        private bool canPickUp;
        private IPickable pickable;

        public void CanPickupPickable(IPickable pickable) {
            canPickUp = true;
        }

        public void CannotPickupPickable(IPickable pickable) {
            canPickUp = false;
        }

        public void PickUpPickable(IPickable pickable) {
            
        }
    }

}
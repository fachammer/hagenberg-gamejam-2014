using UnityEngine;
using System.Collections;

namespace TheDarkNight.Picking {
    public interface IPicker {
        void CanPickupPickable(GameObject pickable);
        void CannotPickupPickable(GameObject pickable);
        void PickUpPickable();
    }
}
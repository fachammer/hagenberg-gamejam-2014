using UnityEngine;
using System.Collections;

namespace TheDarkNight.Picking {
    public interface IPicker {

        void CanPickupPickable(IPickable pickable);
        void CannotPickupPickable(IPickable pickable);
        void PickUpPickable(IPickable pickable);

    }
}
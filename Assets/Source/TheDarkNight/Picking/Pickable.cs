using UnityEngine;
using System.Collections;
using TheDarkNight.Extensions;

namespace TheDarkNight.Picking {
    public class Pickable : MonoBehaviour, IPickable {

        public void OnTriggerEnter(Collider other) {
            IPicker picker = other.GetClass<IPicker>();
            if(picker != null) {
                picker.PickUpPickable(this);
            }
        }

        public void PickUp() {
            //IPicker picker = other.GetClass<IPicker>();
            //if(picker != null) {
            //    picker.AddToInventory(this);
            //}
        }

    }

}
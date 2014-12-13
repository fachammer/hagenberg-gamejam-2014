using UnityEngine;
using System.Collections;
using TheDarkNight.Extensions;

namespace TheDarkNight.Picking {
    public class Pickable : MonoBehaviour, IPickable {

        public void OnTriggerEnter(Collider other) {
            IPicker picker = other.GetClass<IPicker>();
            if(picker != null) {
                picker.CanPickupPickable(this.gameObject);
            }        
        }

        public void OnTriggerExit(Collider other) {
            IPicker picker = other.GetClass<IPicker>();
            if(picker != null) {
                picker.CannotPickupPickable(this.gameObject);
            }
        }
    }
}
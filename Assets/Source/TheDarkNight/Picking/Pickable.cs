using TheDarkNight.Extensions;
using UnityEngine;

namespace TheDarkNight.Picking {

    [RequireComponent(typeof(Collider))]
    public class Pickable : MonoBehaviour, IPickable {

        public Transform GetTransform() {
            return this.transform;
        }

        protected virtual void Start() {
            collider.isTrigger = true;
        }

        protected void OnTriggerEnter(Collider other) {
            IPicker picker = other.GetClass<IPicker>();
            if(picker != null) {
                picker.CanPickupPickable(this);
            }
        }

        protected void OnTriggerExit(Collider other) {
            IPicker picker = other.GetClass<IPicker>();
            if(picker != null) {
                picker.CannotPickupPickable(this);
            }
        }
    }
}
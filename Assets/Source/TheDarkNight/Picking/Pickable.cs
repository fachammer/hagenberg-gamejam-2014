using TheDarkNight.Extensions;
using UnityEngine;

namespace TheDarkNight.Picking {

    [RequireComponent(typeof(Collider))]
    public class Pickable : MonoBehaviour, IPickable {
        private IPicker picker;

        public Transform GetTransform() {
            return this.transform;
        }

        public bool CanBePickedUpBy(IPicker picker) {
            return this.picker == null || picker == this.picker;
        }

        public void CannotBePickedupByOthersThan(IPicker picker) {
            this.picker = picker;
        }

        protected virtual void Start() {
            collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other) {
            IPicker picker = other.GetClass<IPicker>();
            if(picker != null)
                picker.CanPickupPickable(this);
        }

        private void OnTriggerExit(Collider other) {
            IPicker picker = other.GetClass<IPicker>();
            if(picker != null) {
                picker.CannotPickupPickable();
            }
        }
    }
}
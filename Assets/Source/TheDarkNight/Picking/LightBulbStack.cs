using UnityEngine;
using System.Collections;
using System.Linq;
using TheDarkNight.Extensions;
using ModestTree.Zenject;
using TheDarkNight.Lights;

namespace TheDarkNight.Picking {

    [RequireComponent(typeof(InventoryUnlimited))]
    [RequireComponent(typeof(Collider))]
    public class LightBulbStack : MonoBehaviour {

        private IPicker picker;
        private IInventory inventory;

        private void Start() {
            inventory = this.TryGetComponent<InventoryUnlimited>();
            collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other) {
            picker = other.GetClass<IPicker>();
            if(picker != null) {
                ILightBulb lightBulb = inventory.GetItems().Where(i => i is ILightBulb).First() as ILightBulb;
                if(lightBulb != null)
                    picker.CanPickupPickable(lightBulb);
            }
        }

        private void OnTriggerExit(Collider other) {
            picker = other.GetClass<IPicker>();
            if(picker != null) {
                picker.CannotPickupPickable();
            }
        }
    }
}
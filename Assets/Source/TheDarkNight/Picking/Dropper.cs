using UnityEngine;
using System.Collections;
using System.Linq;
using TheDarkNight.Picking;
using TheDarkNight.Extensions;
using TheDarkNight.Lights;
using ModestTree.Zenject;

namespace TheDarkNight.Picking {

    [RequireComponent(typeof(Inventory))]
    public class Dropper : MonoBehaviour, IDropper {
        [Inject]
        public GameObjectInstantiator GOI { get; set; }

        private IInventory inventory;
        
        public bool TryDropLightBulb() {
            ILightBulb lightBulb = inventory.GetItems().Where(i => i is ILightBulb).First() as ILightBulb;
            if(lightBulb != null) {
                GOI.Instantiate(lightBulb.GetTransform().gameObject, transform.position);
                inventory.RemoveItem(lightBulb);
                return true;
            }

            return false;
        }

        private void Start() {
            this.inventory = this.TryGetClass<IInventory>();
        }
    }
}
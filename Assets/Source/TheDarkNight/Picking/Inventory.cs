using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TheDarkNight.Lights;
using TheDarkNight.FlashLight;
using ModestTree.Zenject;

namespace TheDarkNight.Picking {

    public class Inventory : MonoBehaviour, IInventory {

        [SerializeField]
        private int maxBatteries = 3;

        [SerializeField]
        private int maxLightBulbs = 1;

        [SerializeField]
        private Battery battery;

        [Inject]
        public GameObjectInstantiator GOI { get; set; }

        private List<IPickable> batteries = new List<IPickable>();
        private List<IPickable> lightBulbs = new List<IPickable>();

        private void Start() {
            for(int i = 0; i < maxBatteries; i++) {
                AddItem(GOI.Instantiate(battery.gameObject).GetComponent<Battery>());
            }
        }

        public bool AddItem(IPickable pickable) {
            if(pickable is ILightBulb && lightBulbs.Count < maxLightBulbs) {
                lightBulbs.Add(pickable as ILightBulb);
                return true;
            }
            else if(pickable is IBattery && batteries.Count < maxBatteries) {
                batteries.Add(pickable as IBattery);
                return true;
            }
            return false;
        }

        public virtual bool RemoveItem(IPickable pickable) {
            if(pickable is ILightBulb) {
                if(lightBulbs.Remove(pickable as ILightBulb)) {
                    return true;
                }
                return false;
            }
            else if(pickable is IBattery) {
                if(batteries.Remove(pickable as IBattery)) {
                    Destroy(pickable.GetTransform().gameObject);
                    return true;
                }
                return false;
            }
            return false;
        }


        public virtual IEnumerable<IPickable> GetItems() {
            return batteries.Concat(lightBulbs);
        }
    }

}
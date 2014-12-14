using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TheDarkNight.Lights;
using TheDarkNight.FlashLight;

namespace TheDarkNight.Picking {

    public class Inventory : MonoBehaviour, IInventory {

        [SerializeField]
        private int maxBatteries = 3;

        [SerializeField]
        private int maxLightBulbs = 1;

        private List<IPickable> batteries = new List<IPickable>();
        private List<IPickable> lightBulbs = new List<IPickable>();

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
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TheDarkNight.Lights;

namespace TheDarkNight.Picking {
    public class Inventory : MonoBehaviour, IInventory{

        [SerializeField]
        private int maxBatteries = 3;

        [SerializeField]
        private int maxLightBulbs = 1;

        private List<IBattery> batteries = new List<IBattery>();
        private List<ILightBulb> lightBulbs = new List<ILightBulb>();

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

        public bool RemoveItem(IPickable pickable) {
            if(pickable is ILightBulb) {
                if(lightBulbs.Remove(pickable as ILightBulb)) {
                    Destroy(pickable.GetTransform().gameObject);
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
    }

}
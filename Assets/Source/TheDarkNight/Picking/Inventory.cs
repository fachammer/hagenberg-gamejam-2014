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

        private int batteryCount = 0;
        private int lightBulbsCount = 0;

        public bool AddPickable(IPickable pickable) {
            if(pickable is ILightBulb && lightBulbsCount < maxLightBulbs) {
                lightBulbs.Add(pickable as ILightBulb);
                return true;
            }
            else if(pickable is IBattery && batteryCount < maxBatteries) {
                batteries.Add(pickable as IBattery);
                return true;
            }
            return false;
        }

        public bool RemovePickable(IPickable pickable) {
            if(pickable is ILightBulb) {
                return lightBulbs.Remove(pickable as ILightBulb);
            }
            else if(pickable is IBattery) {
                return batteries.Remove(pickable as IBattery);
            }
            return false;
        }
    }

}
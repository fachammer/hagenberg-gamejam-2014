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

        private List<GameObject> batteries = new List<GameObject>();
        private List<GameObject> lightBulbs = new List<GameObject>();

        public bool AddItem(GameObject pickable) {
            if(pickable.GetComponent<LightBulb>() != null && lightBulbs.Count < maxLightBulbs) {
                lightBulbs.Add(pickable);
                return true;
            }
            else if(pickable.GetComponent<Battery>() != null && batteries.Count < maxBatteries) {
                batteries.Add(pickable);
                return true;
            }
            return false;
        }

        public bool RemoveItem(GameObject pickable) {
            if(pickable.GetComponent<LightBulb>() != null) {
                if(lightBulbs.Remove(pickable)){
                    Destroy(pickable.gameObject);
                    return true;
                }
                return false;
            }
            else if(pickable.GetComponent<Battery>() != null) {
                if(batteries.Remove(pickable)) {
                    Destroy(pickable.gameObject);
                    return true;
                }
                return false;
            }
            return false;
        }
    }

}
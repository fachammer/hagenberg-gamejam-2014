using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TheDarkNight.Picking;
using TheDarkNight.Extensions;
using UniRx;

namespace TheDarkNight.FlashLight {
    
    [RequireComponent(typeof(IInventory))]
    public class FlashLight : MonoBehaviour, IFlashLight {
        private bool turnedOn = false;
        private IInventory inventory;
        private List<IBattery> batteries;
        private IBattery batteryInUse;

        private void Start() {
            inventory = GetComponent<Inventory>();            
        }

        public bool TryTurnOn() {
            if(!turnedOn && TryUseNewBattery()) { 
                turnedOn = true;
                return true;
            }
            return false;
        }

        public bool TryTurnOff() {
            if(turnedOn) {
                turnedOn = false;
                return true;
            }
            return false;
        }

        private void Update() {
            if(turnedOn) {
                batteryInUse.DecreaseBatteryTime(Time.deltaTime);
                if(batteryInUse.GetRemainingTime() <= 0) {
                    inventory.RemoveItem(batteryInUse);
                    if(!TryUseNewBattery()) {
                        turnedOn = false;
                    }
                }
            }
        }

        private bool TryUseNewBattery() {
            UpdateBatteries();
            float batteryLoad = batteries.Sum(b => b.GetRemainingTime());

            if(batteries.Count() > 0 && batteryLoad > 0) {
                batteryInUse = batteries.Where(b => b.GetRemainingTime() > 0).First();
                return true;
            }
            return false;
        }

        private void UpdateBatteries() {
            batteries = inventory.GetItems().Where(item => item is IBattery) as List<IBattery>;
        }

        private void DrawLight(bool lightEnabled) {
            Light light = this.TryGetComponentsInChildren<Light>().First();
            light.enabled = lightEnabled;
            GameObject lightCone = this.TryGetComponentsInChildren<Transform>().Where(g => g.tag == Tags.LIGHT_CONE).First().gameObject;
            lightCone.SetActive(lightEnabled);
        }
    }
}

using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TheDarkNight.Picking;
using TheDarkNight.Extensions;
using UniRx;

namespace TheDarkNight.FlashLight {
    
    public class FlashLight : MonoBehaviour, IFlashLight {
        private bool turnedOn = false;
        private IInventory inventory;
        private List<IBattery> batteries;
        private IBattery batteryInUse;

        private void Start() {
            inventory = this.TryGetComponentInParent<Inventory>();
        }

        public bool TryTurnOn() {
            if(!turnedOn && TryUseNewBattery()) {
                DrawLight(true);
                turnedOn = true;
                return true;
            }
            return false;
        }

        public bool TryTurnOff() {
            if(turnedOn) {
                DrawLight(false);
                turnedOn = false;
                return true;
            }
            return false;
        }

        public void Toggle() {
            if(!TryTurnOff()) {
                TryTurnOn();
            }
        }

        private void Update() {
            if(turnedOn) {
                batteryInUse.DecreaseBatteryTime(Time.deltaTime);
                if(batteryInUse.GetRemainingTime() <= 0) {
                    inventory.RemoveItem(batteryInUse);
                    if(!TryUseNewBattery()) {
                        TryTurnOff();
                    }
                }
            }
        }

        private bool TryUseNewBattery() {
            UpdateBatteries();

            if(batteries == null || batteries.Count() <= 0)
                return false;

            float batteryLoad = batteries.Select(b => b.GetRemainingTime()).Sum();

            if(batteryLoad <= 0)
                return false;
            
            batteryInUse = batteries.Where(b => b.GetRemainingTime() > 0).First();
            return true;
        }

        private void UpdateBatteries() {
            batteries = inventory.GetItems().Where(item => item is IBattery).Select(item => item as IBattery).ToList();
        }

        private void DrawLight(bool lightEnabled) {
            Light light = this.TryGetComponent<Light>();
            light.enabled = lightEnabled;
            GetComponentsInChildren<MeshRenderer>().First().enabled = lightEnabled;
            GetComponentsInChildren<DarknessHider>().First().enabled = lightEnabled;
        }
    }
}

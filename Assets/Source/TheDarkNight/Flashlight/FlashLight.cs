using System.Collections.Generic;
using System.Linq;
using TheDarkNight.Darkness;
using TheDarkNight.Extensions;
using TheDarkNight.Lights;
using TheDarkNight.Picking;
using UnityEngine;

namespace TheDarkNight.FlashLight {

    public class FlashLight : MonoBehaviour, IFlashLight {
        private bool turnedOn = false;
        private IInventory inventory;
        private List<IBattery> batteries;
        private IBattery batteryInUse;

        public bool TryTurnOn() {
            if(inventory.GetItems().Where(i => i is ILightBulb).Count() > 0)
                return false;

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
            if(!TryTurnOn())
                TryTurnOff();
        }

        public float GetBatteryLoad() {
            UpdateBatteries();
            return batteries != null ? batteries.Select(b => b.GetRemainingTime()).Sum() : 0f;
        }

        private void Start() {
            inventory = this.TryGetComponentInParent<Inventory>();
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
            audio.Play();
            Light light = this.TryGetComponentsInChildren<Light>().First();
            light.enabled = lightEnabled;
            GetComponentsInChildren<MeshRenderer>(true).First().enabled = lightEnabled;
            GetComponentsInChildren<DarknessKillerFlashlight>(true).First().collider.enabled = lightEnabled;            
        }
    }
}

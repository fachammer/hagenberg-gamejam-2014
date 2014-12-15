using UnityEngine;
using System.Collections;
using System.Linq;
using TheDarkNight.Picking;
using TheDarkNight.Extensions;

namespace TheDarkNight.FlashLight {
    public class Battery : Pickable, IBattery {
        [SerializeField]
        private float runTimeSeconds = 5f;

        public float GetRemainingTime() {
            return runTimeSeconds;
        }

        public void DecreaseBatteryTime(float time) {
            runTimeSeconds -= time;            
        }
    }
}
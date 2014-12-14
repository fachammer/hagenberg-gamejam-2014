using UnityEngine;
using System.Collections;
using System.Linq;
using TheDarkNight.Picking;
using TheDarkNight.Extensions;

namespace TheDarkNight.FlashLight {
    public class Battery : Pickable, IBattery {
        [SerializeField]
        private float runTimeSeconds = 5f;

        private void Start() {
            //transform.GetComponentsInChildren<Transform>().Where(t => t.gameObject.name != this.gameObject.name).Do(t => t.transform.rotation = Quaternion.identity);
        }

        public float GetRemainingTime() {
            return runTimeSeconds;
        }

        public void DecreaseBatteryTime(float time) {
            runTimeSeconds -= time;
        }
    }
}
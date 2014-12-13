using UnityEngine;
using System.Collections;

namespace TheDarkNight.FlashLight {
    public class Battery : MonoBehaviour, IBattery {


        public Transform GetTransform() {
            return this.transform;
        }
    }
}
using UnityEngine;
using System.Collections;

namespace TheDarkNight.FlashLight {
    public class Battery : MonoBehaviour, IBattery {
        [SerializeField]
        private float runTimeSeconds = 5f;

        public Transform GetTransform() {
            return this.transform;
        }

        public float GetRemainingTime() {
            return runTimeSeconds;
        }
    }
}
using UnityEngine;
using System.Collections;

namespace TheDarkNight.Lights {
    public class LightBulb : MonoBehaviour, ILightBulb {
        [SerializeField]
        private Light pointLight;

        private bool intact;

        public bool TryTurnOn() {
            if(intact && !pointLight.enabled) {
                pointLight.enabled = true;
                return true;
            }
            return false;
        }

        public bool TryTurnOff() {
            if(intact && pointLight.enabled) {
                pointLight.enabled = false;
                return true;
            }
            return false;
        }

        public void Destroy() {
            TryTurnOff();
            intact = false;
        }
    }

}